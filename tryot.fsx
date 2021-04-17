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