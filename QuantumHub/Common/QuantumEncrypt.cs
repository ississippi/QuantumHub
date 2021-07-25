using System;
using System.Security.Cryptography;
using System.Text;

namespace QuantumHub.Common
{
    public static class QuantumEncrypt
    {
        const int CIPHER_VERSION_LEN = 2;
        const int CIPHER_SERIAL_NO_LEN = 75;
        const int CIPHER_START = CIPHER_VERSION_LEN + CIPHER_SERIAL_NO_LEN;
        const int CIPHER_RESERVED_BYTES = CIPHER_VERSION_LEN + CIPHER_SERIAL_NO_LEN;
        const int CIPHER_START_LOCATION_LEN = 25;
        const int ENCRYPTED_FILE_PREFIX = CIPHER_SERIAL_NO_LEN + CIPHER_START_LOCATION_LEN;

        public static byte[] Encrypt(string fileName, byte[] arrToEncrypt, string cipher, int cipherEncryptStart, string serialNo, IProgress<int> progress, ref string reason)
        {
            // 1. Validate
            if (!IsValidForEncryption(arrToEncrypt, cipher, cipherEncryptStart, ref reason))
                return null;

            // 2. Build New Array
            // Encrypted File Format:
            // +------------------------------+------------------------+------------------------+------------------------------------+
            // | 75 Byte Serial Number        | 25 Byte Location in    | Encrypted original     | Encrypted file bytes               |
            // |                              | cipher for encryption  | filename ending in ":" |                                    |
            // +------------------------------+------------------------+------------------------+------------------------------------+                              |             
            //
            fileName += ":";  //filename delimiter ":"
            var result = new byte[GetEncryptedFileLen(arrToEncrypt.Length, fileName.Length)];
            var workingArray = new byte[arrToEncrypt.Length + fileName.Length];
            var idxResult = 0;
            var fileNameArr = new byte[fileName.Length];
            var idxFNArr = 0;
            CopyStringToByteArray(fileName, ref fileNameArr, ref idxFNArr);
            fileNameArr.CopyTo(workingArray, 0);
            arrToEncrypt.CopyTo(workingArray, fileName.Length);
            var cipherEncryptionBegin = CIPHER_RESERVED_BYTES + cipherEncryptStart;
            var cipherStartLocation = cipherEncryptionBegin.ToString("D25");


            //Encrypt filename + file data
            var encryptedBytes = ECDC(workingArray, 0, cipher, cipherEncryptionBegin, workingArray.Length, progress);


            CopyStringToByteArray(serialNo, ref result, ref idxResult);
            CopyStringToByteArray(cipherStartLocation, ref result, ref idxResult);
            encryptedBytes.CopyTo(result, idxResult);

            return result;
        }
        public static byte[] Decrypt(byte[] arr, string cipher, int cipherEncryptionBegin, IProgress<int> progress, ref string reason)
        {
            var newArrayLen = arr.Length - ENCRYPTED_FILE_PREFIX;
            if (!IsValidForDecryption(arr, cipher, newArrayLen, ref reason))
                return null;

            // Decrypt and remove filename from the unencrypted byte array
            var unencryptedWithFilename = ECDC(arr, ENCRYPTED_FILE_PREFIX, cipher, cipherEncryptionBegin, newArrayLen, progress);
            var newArray = StripFileName(unencryptedWithFilename);

            return newArray;
        }
        private static byte[] ECDC(byte[] arr, int idxArr, string cipher, int cipherEncryptionBegin, int newArrayLen, IProgress<int> progress = null)
        {
            int idxCipher = cipherEncryptionBegin;
            var result = new byte[newArrayLen];
            var idxResult = 0;
            var amountToEncrypt = arr.Length - idxArr;
            for (int idx = idxArr; idx < arr.Length; idx++)
            {
                int y = ((int)cipher[idxCipher] ^ (int)arr[idx]);
                result[idxResult] += ((byte)y);
                idxCipher++;
                idxResult++;
                if (progress != null)
                {
                    int percentComplete = (idxResult * 100) / amountToEncrypt;
                    progress.Report(percentComplete);
                }
            }
            return result;
        }

        private static bool IsValidForEncryption(byte[] arr, string cipher, int cipherEncryptStart, ref string reason)
        {
            var isValid = true;
            if (arr == null || arr.Length < 1)
            {
                reason += "File to encrypt is not loaded";
                isValid = false;
            }
            if (string.IsNullOrEmpty(cipher))
            {
                reason += $"\nCipher is not loaded.";
                isValid = false;
            }
            else
            {
                var usableCipherLen = cipher.Length - (CIPHER_RESERVED_BYTES + cipherEncryptStart);
                if (arr.Length > usableCipherLen)
                {
                    reason += $"\nCipher not large enough to encrypt file. ";
                    if (arr.Length < (cipher.Length - CIPHER_RESERVED_BYTES))
                        reason += $"\nConsider lowering the cipher encrypt start location input value.";
                    reason += $"\n\nFile to encrypt length: {arr.Length}\nCipher File Length: {cipher.Length}\nUsable Cipher Bytes: {usableCipherLen}\nCipher Encrypt Start Location: {cipherEncryptStart}";
                    isValid = false;
                }
            }

            return isValid;
        }

        private static bool IsValidForDecryption(byte[] arr, string cipher, int newArrayLen, ref string reason)
        {
            var isValid = true;
            if (arr == null || arr.Length < 101)
            {
                reason += "File to decrypt is not loaded";
                isValid = false;
            }

            if ((string.IsNullOrEmpty(cipher)) || newArrayLen > (cipher.Length - CIPHER_START))
            {
                reason += $"\nCipher not large enough to decrypt file. Cipher: {cipher.Length - CIPHER_START} File: {newArrayLen}";
                isValid = false;
            }
            var cipherSerial = string.Empty;
            var encryptedSerial = string.Empty;
            if (!IsSerialNoMatchForDecryption(arr, cipher, ref cipherSerial, ref encryptedSerial))
            {
                reason += $"\nSerial numbers do not match. Cipher: {cipher.Length - CIPHER_START} File: {newArrayLen}";
                isValid = false;
            }
            return isValid;
        }

        public static int GetCipherStartLocation(byte[] arrEncrypted)
        {
            var strLocation = CopyBytesToString(arrEncrypted, CIPHER_SERIAL_NO_LEN, CIPHER_START_LOCATION_LEN);
            var startLocation = -1;
            Int32.TryParse(strLocation, out startLocation);

            return startLocation;
        }
        public static void CopyStringToByteArray(string str, ref byte[] arrDestination, ref int idxDestination)
        {
            foreach (char c in str.ToCharArray())
            {
                arrDestination[idxDestination++] = (byte)c;
            }
        }
        public static string CopyBytesToString(byte[] arr, int idx, int len)
        {
            var newArr = new byte[len];
            var endLoc = idx + len;
            var j = 0;
            for (int i = idx; i < endLoc; i++)
            {
                newArr[j++] = arr[i];
            }
            var str = Encoding.Default.GetString(newArr);

            return str;
        }

        public static int GetEncryptedFileLen(int fileToEncryptLen, int fileNameLen)
        {
            return fileToEncryptLen + 100 + fileNameLen;
        }

        public static int GetCipherLen(int fileToEncryptLen)
        {

            return CIPHER_START + fileToEncryptLen;
        }
        public static int GetMaxFileSizeForEncryption(string cipherString)
        {
            return cipherString.Length - CIPHER_START;
        }
        public static string GetSerialNumberFromEncryptedBytes(byte[] encryptedBytes)
        {
            return CopyBytesToString(encryptedBytes, 0, CIPHER_SERIAL_NO_LEN);
        }

        public static string GetSerialNumberFromCipher(string cipherString)
        {
            return cipherString.Substring(2, CIPHER_SERIAL_NO_LEN);
        }
        public static string GetVersionNumberFromCipher(string cipherString)
        {
            return cipherString.Substring(0, CIPHER_VERSION_LEN);
        }

        public static string GenerateRandomCryptographicKey(int keyLength)
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes).Substring(0, keyLength);
            //return Encoding.Default.GetString(randomBytes);
        }

        private static byte[] StripFileName(byte[] arr)
        {
            int idxInArr;
            for (idxInArr = 0; idxInArr < arr.Length; idxInArr++)
            {
                if (arr[idxInArr] == 0x3A)
                    break;
            }
            idxInArr++;
            var newArray = new byte[arr.Length - (idxInArr)];
            for (int j = 0; j < newArray.Length; j++)
            {
                newArray[j] = arr[idxInArr++];
            }
            return newArray;
        }

        public static bool IsSerialNoMatchForDecryption(byte[] encryptedBytes, string cipher, ref string cipherSerial, ref string bytesSerial)
        {
            cipherSerial = GetSerialNumberFromCipher(cipher);
            bytesSerial = GetSerialNumberFromEncryptedBytes(encryptedBytes);

            return (cipherSerial == bytesSerial);
        }
        /// <summary>
        /// Create a Formatted Display of the input byte array returning as a string.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bytesPerLine"></param>
        /// <returns></returns>
        public static string HexDump(string hexThis)
        {
            var hexThisArray = new byte[hexThis.Length];
            var idxByteArray = 0;
            CopyStringToByteArray(hexThis, ref hexThisArray, ref idxByteArray);
            return HexDump(hexThisArray);
        }

        public static string HexDump(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null) return "<null>";
            int bytesLength = bytes.Length;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                  8                   // 8 characters for the address
                + 3;                  // 3 spaces

            int firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            int lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(i >> 28) & 0xF];
                line[1] = HexChars[(i >> 24) & 0xF];
                line[2] = HexChars[(i >> 20) & 0xF];
                line[3] = HexChars[(i >> 16) & 0xF];
                line[4] = HexChars[(i >> 12) & 0xF];
                line[5] = HexChars[(i >> 8) & 0xF];
                line[6] = HexChars[(i >> 4) & 0xF];
                line[7] = HexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = (b < 32 ? '·' : (char)b);
                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);
            }
            return result.ToString();
        }
    }
}
