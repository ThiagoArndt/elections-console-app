# elections-console-app
This is a console application made to simulate elections.

To start this application you need to create a MySQL database and connect it with VisualStudio, after that you are ready to go!
To create the database, run this command:

~~~
CREATE DATABASE dbCRUD
USE dbCRUD

CREATE TABLE Elections(user_id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
                    user_name VARCHAR(50) NOT NULL,
                    user_age INT NOT NULL,
                    user_tel INT NOT NULL,
                    user_cpf VARCHAR(14) NOT NULL,
                    user_voto VARCHAR(50) NOT NULL,
                    )
~~~

After that, you can open VisualStudio and connect to your MySQL server.
Also, remember to change the name of your server and the name of the database created on the application code.


