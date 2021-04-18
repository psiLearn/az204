#r "System.Net.WebClient.dll"

open System
open System.Net
open System.IO
open System.Diagnostics

let loadUp() = 
    let webAppName  = Environment.GetEnvironmentVariable("TEMP_WEB_APP_NAME", EnvironmentVariableTarget.User)
    printfn "updating: %A" webAppName
    let fileName =  Path.GetTempFileName() 
                    |> fun n -> [   //n |> Path.GetDirectoryName
                                    n |> Path.GetFileNameWithoutExtension
                                    ".zip"  ] |> List.reduce (+) 
    printfn "temp file: %A"  fileName  
    let url  = @"https://github.com/MicrosoftLearning/AZ-204-DevelopingSolutionsforMicrosoftAzure/blob/master/Allfiles/Labs/01/Starter/API/api.zip?raw=true"
    let webClient = new WebClient()
    printfn "starting downloading"  
    webClient.DownloadFile (Uri url, fileName)
    // let req = url |> Uri |> WebRequest.Create
    // use resp = req.GetResponse()
    // use stream = resp.GetResponseStream()
    // use writer = new FileStream(fileName, FileMode.Create,  FileAccess.Write)
    // writer.
    printfn "starting uploading"  
    let pars = sprintf "webapp deployment source config-zip --resource-group ManagedPlatform --src %A --name %s" fileName webAppName
    printfn "az %s" pars  
    let az = Process.Start("az", pars)
    az.WaitForExit()
    File.Delete fileName
loadUp()



#r "nuget: Azure.Storage.Blobs"

open System
open System.Net
open System.IO
open Azure 
open Azure.Storage
open Azure.Storage.Blobs

let connectionString = System.Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING")

let client = BlobServiceClient(connectionString);
let containerClient = client.GetBlobContainerClient("images")

let uploadExample () =
    let url  = "https://github.com/MicrosoftLearning/AZ-204-DevelopingSolutionsforMicrosoftAzure/blob/master/Allfiles/Labs/01/Starter/Images/grilledcheese.jpg?raw=true"
    let req = WebRequest.Create(Uri(url))
    use resp = req.GetResponse()
    use stream = resp.GetResponseStream()
    
    containerClient.UploadBlob("Grillcheese.jpg",stream)
uploadExample () 