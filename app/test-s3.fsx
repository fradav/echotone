#r "nuget: dotenv.net"
#r "nuget: AWSSDK.S3"

open dotenv.net
open System.IO
open Amazon.S3
open Amazon.S3.Model
open Amazon.S3.Util
open System

let sourceDir = Path.Combine(__SOURCE_DIRECTORY__, "..")

DotEnv.Load(DotEnvOptions(envFilePaths = [ Path.Combine(sourceDir, ".env-secret.local") ]))

let config =
    DotEnv.Read(DotEnvOptions(envFilePaths = [ Path.Combine(sourceDir, ".env-app") ]))


let s3 =
    new AmazonS3Client(
        Environment.GetEnvironmentVariable("S3_ACCESS_KEY_ID"),
        Environment.GetEnvironmentVariable("S3_SECRET_ACCESS_KEY"),
        Amazon.RegionEndpoint.GetBySystemName(config["S3_REGION"])
    )

let s3Bucket = config["S3_BUCKET"]

let s3ObjectRequest =
    new Amazon.S3.Model.GetObjectMetadataRequest(BucketName = s3Bucket, Key = "performance-nicolas-bralet-extrait1.mp4")

let s3ObjectResponse = s3.GetObjectMetadataAsync(s3ObjectRequest).Result
s3ObjectResponse
let s3ObjectRequest = Model.GetObjectRequest(BucketName = s3Bucket, Key = "performance-nicolas-bralet-extrait1.mp4", ChecksumMode = ChecksumMode.ENABLED)
let s3ObjectResponse = s3.GetObjectAsync(s3ObjectRequest).Result
s3ObjectResponse

// list all objects in the bucket
let listRequest = new Amazon.S3.Model.ListObjectsV2Request(BucketName = s3Bucket)
let listResponse = s3.ListObjectsV2Async(listRequest).Result
listResponse.S3Objects
|> Seq.iter (fun obj -> printfn "%s" obj.)

#r "fsproj: site-gen/SiteGen.fsproj"

open Site

let vid = 
    Gen.AssetsT.GetSample().Items
    |> Seq.find (fun x -> x.Slug = "performance-nicolas-bralet-extrait1.mp4")

let refurl = Gen.uploadToS3 vid

