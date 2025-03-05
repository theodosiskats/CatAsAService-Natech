# 🐱 CatsAsAService

This project is a demonstration of skills. Yes, I know, fetching cat images shouldn’t be that complicated, yet I believe the “overkill” approach is a good opportunity to explore how I tackle more complex tasks.

## 🚀 Features & Infrastructure

This project utilizes several remote APIs and services to achieve its functionality. The following infrastructure components are involved:

- **Azure Functions** - Handles image resizing
- **TheCatApi.com** - Provides random cat images
- **AWS S3** - Stores images
- **MSSQL Server** - Manages the database

## 🛠️ How to Test the Project

There are **two ways** to test the functionality of this project:

### 1️⃣ Using My Public Infrastructure (Easiest)
You can use my publicly available S3 buckets, Azure functions, etc., by configuring the provided `local.settings.json`. If you haven’t received my settings, please contact me, and I will share them with you.

### 2️⃣ Hosting Everything on Your Own
To set up your own environment, follow these steps:

1. **Get an API Key** from [TheCatApi](https://thecatapi.com/).
2. **Set up an AWS S3 Bucket** and make it publicly accessible.
3. **Deploy an Azure Function** to handle image compression.
4. **Configure your database** (local or remote MS SQL Server) with the name `catsdb-kt`.

## ⚙️ AppSettings Configuration

Your `appsettings.json` should be configured as follows:

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "LocalDB": "Server=.;Database=catsdb-kt;User Id=admin;Password=admin;",
    "RemoteDB": "Server=YOUR_SERVER_IP;Database=catsdb-kt;User Id=SA;Password=admin@1234;TrustServerCertificate=True;Connection Timeout=30",
    "CatApiUrl": "https://api.thecatapi.com/v1/images/search?mime_types=jpg&has_breeds=true&order=RANDOM&limit=25",
    "CatApiKey": "DEMO-API-KEY",
    "AWSAccessKey": "YOUR_AWS_KEY",
    "AWSSecretKey": "YOUR_AWS_SECRET",
    "AWSRegion": "eu-central-1",
    "AwsBucketName": "natech-cats",
    "ImageCompressFunctionUrl": "https://YouAzureFunction.azurewebsites.net/api/ImageResizeFunction"
}
```

## 🏗️ Setting Up the Database

Ensure that you have created a new database named **`catsdb-kt`** on either:
- Your **localhost** (MS SQL Server)
- A remote **MS SQL Server**

You might notice that there are 2 connection strings in the above-mentioned settings. One is for local dbs and the other is for remote db.
Choose whichever suits your test case best and replace in the API/Program.cs the connString with the required environment variable's name.

## 🐳 Running the Project

Once all dependencies are set up, you can run the project using one of the following methods:

### 🏃‍♂️ Running via Docker

Execute the following command in the root folder of the solution:
```sh
docker build -t catasaservice .
docker run -p 8080:8080 --name catasaservice catasaservice
```

### 🛠️ Running in Debug Mode
Run the project using your favorite IDE (such as **Visual Studio**, **Rider**, or **VS Code**) in **Debug Mode**.
If provided, before running, copy your local.settings.json file into the API projects folder.
---

## 🎯 Conclusion
This project demonstrates the integration of multiple cloud services and APIs in a single application. While fetching cat images is a simple task, this implementation showcases a structured, scalable, and extensible approach that can be applied to more complex systems.

The execution time of the method fetch, depends on the network speed you have. As 25 images per request is a considerable size, the request may take up to 30 seconds to execute. Yet when we run it on a deployed instance in a high network speed server, the request is done in 15 seconds.

Feel free to reach out if you have any questions! 🚀🐱