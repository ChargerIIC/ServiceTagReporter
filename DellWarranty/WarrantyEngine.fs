module DellWarranty.WarrantyEngine

open Microsoft.FSharp.Data.TypeProviders
open System.Management
open System
open System.Data
open System.Reflection
open System.IO

//type providers
type dellService = WsdlService<"http://xserv.dell.com/services/AssetService.asmx?WSDL">

//Objects
let AppID = new Guid("{C8559A27-E1BD-40A7-85AF-C0624AC8E5C4}")

let assertService = dellService.GetAssetServiceSoap()

let dataTable = new DataTable("Data")
dataTable.Columns.Add("ServiceTag") |> ignore
dataTable.Columns.Add("Model") |> ignore
dataTable.Columns.Add("Purchase Date") |> ignore
dataTable.Columns.Add("Warranty Date") |> ignore
    

let FetchTag(tag: string) = 
    if not (String.IsNullOrEmpty tag) then
        let warranty = assertService.GetAssetInformation(AppID, "Service Tag Reporter", tag)
        if (warranty.Length > 0) then
            let newRow = dataTable.NewRow();   
            newRow.["ServiceTag"] <- tag;
            newRow.["Model"] <- warranty.[0].AssetHeaderData.SystemModel;
            let possibleWarranties = warranty.[0].Entitlements |> Seq.filter(fun x -> (not (x.ServiceLevelDescription.Contains("Digitial Delivery"))))
            newRow.["Purchase Date"] <- (possibleWarranties |> Seq.minBy(fun x -> x.StartDate)).StartDate
            newRow.["Warranty Date"] <- (possibleWarranties |> Seq.minBy(fun x -> x.EndDate)).EndDate 
            dataTable.Rows.Add(newRow);