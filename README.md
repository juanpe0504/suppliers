# Suppliers

.NET Core 2.1 application for suppliers and supplierRates. 
The structure of the projects look like the image below.

![image](https://user-images.githubusercontent.com/6061536/56471977-365fcf80-642f-11e9-9521-98550ba78695.png)

- [Suppliers](https://github.com/juanpe0504/suppliers/tree/master/Suppliers) (API Interface)
- [Suppliers.Services](https://github.com/juanpe0504/suppliers/blob/master/Suppliers.Services) (Business Layer)
- [Suppliers.Providers](https://github.com/juanpe0504/suppliers/tree/master/Suppliers.Providers) (Data Access Layer)
- [Suppliers.Domain](https://github.com/juanpe0504/suppliers/tree/master/Suppliers.Domain) (Application Domain)

## Exercises

The solution of first exercise is [here](https://github.com/juanpe0504/suppliers/blob/master/scripts/createAndPopulate.sql).

The models of second exersice [Supplier](https://github.com/juanpe0504/suppliers/blob/master/Suppliers.Domain/Models/Supplier.cs) and [SupplierRate](https://github.com/juanpe0504/suppliers/blob/master/Suppliers.Domain/Models/SupplierRate.cs).

The third will be found in [SupplierService](https://github.com/juanpe0504/suppliers/blob/master/Suppliers.Services/SupplierService.cs) method **AddSupplierRate**

## Running

```
 git clone https://github.com/juanpe0504/suppliers.git
```
Open .sln file with Visual Studio 2017, restore all nuget packages and play IIS Express server. 
The application is using Swagger for documentations of models, responses and endpoints.


#### For running tests and Coverage:

It is assumed that you have installed **dotnet** as command.

In the root folder, you will found runCoverage.ps1 file. Open powershell console and run it .\runCoverage.ps1.

After this procedure complete, the report summary HTML will be opened.

If not, you can find it on .\CodeCoverageHTML folder.


Other way, open the Test Explorer window on Visual Studio and Run All Tests. (this way will not generate the CodeCoverageHTML folder)
