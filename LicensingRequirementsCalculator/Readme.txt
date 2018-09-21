-------------- Licensing Requirements Calculator --------------

Description:
I have used a simple console app to demonstrate the CalculateLicensingRequirements code.
It works on the three examples provided and also on the provided file "sample-small.csv".

Limitations:
It throws an out of memory exception on "sample-large.csv". In a real-world scenario, I would first try to identify all opportunities to reduce the size of the incoming file or to receive it in smaller individual files, possibly per Application or group of Applications.
After exhausting that option, if we do have to work with file sizes in GBs, one way to do this is to write C# code to bulk load it into an sql database table and write CalculateLicensingRequirements code in sql.
I tried it manually; the bulk upload took a few minutes and the query took between 10-15 seconds on my laptop, and this is without any indexes yet. 
Please find attached sql code and output files ("CalculateLicensingRequirements.sql" and "sample-large-licensing-requirements.csv" respectively).
Before the sql solution, I did consider the option of breaking the file into row-wise chunks before loading into memory, but that fails to address the duplicates issue.

As a Service:
I would set up a secure FTP location where these files can be uploaded, and the code would sit in a Windows Service that checks for the presence of any files on FTP, processes them, and puts the results file back on FTP in Output folders.
If the input file sizes were not prohibitively large, I would have preferred to do a Web Api, ideally accepting the Installations List in XML/JSON, and returning the results to caller right there.
Depending upon the real-time load we need to handle, either we can lock that table for each execution or create and drop a new table for each request.