# What is it about?
This repository provides a project for creating an inverted index. The project consists of three logical parts:
1) a class library that allows you to create an inverted index and tests for it;
2) a console application for testing the speed of work and checking the correctness of the parallel algorithm;
3) a client-server application for demonstrating the work of the library.

# Project architecture
 - ```namespace Indexing.BL``` - base library of inverted indexing;
 - ```namespace Indexing.BL.Tests``` - unit test for library
 - ```namespace Indexing.TimeAnalytics``` - console application with performance test
 - ```namespace Indexing.Application``` - library with common classes for client and server
 - ```namespace Indexing.Application.Client``` - client part of client-server application
 - ```namespace Indexing.Application.Server``` - server part of client-server application
 
 ![image](https://user-images.githubusercontent.com/74730992/119378018-6c36a880-bcc6-11eb-90e0-1287cc9b5604.png)

# Dependencies
- ### ```Indexing.BL```
  no dependencies
- ### ```Indexing.BL.Tests```
  - ```Indexing.BL```
  - ```coverlet.collector```
  - ```Microsoft.NET.Test.Sdk```
  - ```xunit```
  - ```xunit.runner.visualstudio```
- ### ```Indexing.TimeAnalytics```
  - ```Indexing.BL```
- ### ```namespase Indexing.Application```
  - ```Newtonsoft.Json```
- ### ```Indexing.Application.Client```
  - ```Indexing.BL```
  - ```Indexing.Application```
- ### ```Indexing.Application.Server```
  - ```Indexing.BL```
  - ```Indexing.Application```

# How to run the project
> [.NET CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/) must be installed to build and run the project.

### 1. Clone or download the solution folder
  Use ```$ git clone https://github.com/VadymKolesnyk/indexer.git``` command in terminal or download .zip archive by [this link](https://github.com/VadymKolesnyk/indexer/archive/refs/heads/main.zip) 
### 2. Run the TimeAnalytics application
  - Go to the solution folder and run the TimeAnalytics project by command ```$ dotnet run --project Indexing/Indexing.TimeAnalytics/Indexing.TimeAnalytics.csproj``` *(The project will be compiled and built automatically)*  
  - Then you will need to enter the path of the folder whose files will be indexed and performance information will be shown
### 3. Run client-server application
  - First you will need to start the server. You can do this with ```$ dotnet run --project Indexing/Indexing.Application.Server/Indexing.Application.Server.csproj  -- arg1 arg2```, where ```arg1```- number of threads used to create the index and ```arg2``` - path to root directory of files.
  - Then you will need to wait for the index creation to complete. 
  - When the server reports that it is running, start the client using the command ```$ dotnet run --project Indexing/Indexing.Application.Client/Indexing.Application.Client.csproj```
  - Then on the client you will need to enter the words you want to find in files.
