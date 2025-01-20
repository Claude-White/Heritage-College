# Assignment 3 - Claude White

## Instructions

1. Clone the repository
2. Open SSMS and connect to your account
3. In SSMS open the sql script **"Assignment2_ClaudeWhite.sql"** found in the repository
4. If required change all instances of **"H60Assignment2DB_CW"** to preferred database name
5. Run the script
6. Open the solution in Visual Studio
7. In all 3 appsettings.json files change the connection string to your database connection string
8. Run the Services project first, then the 2 other projects in whatever order you prefer
9. Have fun exploring my website!

## Docker Instructions

1. In terminal go to the directory with the docker-compose.yaml file
2. Run **"docker compose build"**
3. Run **"docker compose up"** (Store will crash)
4. Open database in SSMS using the credentials below
5. Run the sql script **"Assignment2_ClaudeWhite.sql"** found in the repository
6. Have fun exploring my website!

## Docker DB Credentials
Server: localhost,5119

User Name: SA

Password: password@12345#

Trust Server Certificate: True

## URLS
Store: http://localhost:6117

Customer: http://localhost:6118

Web API: http://localhost:5115 (for this you need to go to /api/Products/Manager for example, because swagger is disabled for production deployment)

Manager: http://localhost:3000

## Users
**Customer**: customer@gmail.com ?Customer1!  
**Clerk**: clerk@gmail.com ?Clerk1!  
**Manager**: manager@gmail.com ?Manager1!
