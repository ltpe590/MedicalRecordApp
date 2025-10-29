const mysql = require('mysql');

const connection = mysql.createConnection({
    host: 'localhost',
    user: 'root', // Your MySQL username
    password: 'root', // Your MySQL root password
    database: 'medrecord' // Your database name
});
