$http = [System.Net.HttpListener]::new()
$http.Prefixes.Add("http://localhost:8080/")
$http.Start()

$root = "$PSScriptRoot"

Write-Host "Serving $root on http://localhost:8080/ ..."

while ($http.IsListening) {
    $context = $http.GetContext()
    $request = $context.Request
    $response = $context.Response

    $path = $root + $request.Url.LocalPath.Replace('/', '\')
    
    if ([System.IO.Directory]::Exists($path)) {
        $path += "index.html"
    }

    if ([System.IO.File]::Exists($path)) {
        $bytes = [System.IO.File]::ReadAllBytes($path)
        $response.ContentLength64 = $bytes.Length
        
        $ext = [System.IO.Path]::GetExtension($path)
        switch ($ext) {
            ".html" { $response.ContentType = "text/html" }
            ".js"   { $response.ContentType = "application/javascript" }
            ".css"  { $response.ContentType = "text/css" }
            ".png"  { $response.ContentType = "image/png" }
            ".jpg"  { $response.ContentType = "image/jpeg" }
            ".mp3"  { $response.ContentType = "audio/mpeg" }
            ".txt"  { $response.ContentType = "text/plain" }
        }
        
        $response.OutputStream.Write($bytes, 0, $bytes.Length)
    } else {
        $response.StatusCode = 404
    }
    
    $response.Close()
}
