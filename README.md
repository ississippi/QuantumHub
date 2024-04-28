<h2>QuantumHub and QuantumConsole Setup Steps</h2>

1. Unzip the files.
2. Create a MySQL database named "quantumencrypt"
3. Launch a MySQL query tool and run the .sql files found in the SQL folder.
4. Open the QuantumHub solution into VS2022 Community or better.
5. Build it.
6. Open the QuantumConsole solution into VS2022 Community or better.
7. Build it.
   
<h3>Test Connections to the DB and to the QuantumHub APIs:</h3>

1. Launch QuantumHub using the VS2022 debugger.
3. Launch QuantumConsole using the VS2022 debugger.
4. Select a user from the "Username" dropdown at the top, and set a max cipher size.
5. Select "Get a New Cipher from Hub".
   a file save dialog will launch. You can cancel or shoose a location and filename for the cipher.
6. A hex display of the cipher will display in the center text area.

<h3>Troubleshooting</h3>
1. Verify the data base tables have been created and all 3 tables have data.
2. Run the QuantumHub unit tests to check the database connection. Troubleshoot database connection issues.
3. Run QuantumConsole and put a breakpoint in the GetNewCipher method.
4. Validate connectivity with the Hub and troubleshoot if needed.
