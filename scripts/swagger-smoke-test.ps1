param(
    [string]$BaseUrl = "https://localhost:7158"
)

$ErrorActionPreference = "Stop"
$stamp = [DateTimeOffset]::UtcNow.ToUnixTimeSeconds()

function Invoke-Api {
    param(
        [Parameter(Mandatory = $true)][string]$Method,
        [Parameter(Mandatory = $true)][string]$Path,
        [object]$Body = $null
    )

    $uri = "$BaseUrl$Path"
    if ($null -eq $Body) {
        return Invoke-RestMethod -Method $Method -Uri $uri
    }

    return Invoke-RestMethod `
        -Method $Method `
        -Uri $uri `
        -ContentType "application/json" `
        -Body ($Body | ConvertTo-Json -Depth 5)
}

Write-Host "1/9 Creating department..."
$department = Invoke-Api POST "/api/Department" @{
    name = "Smoke Test Department $stamp"
}

Write-Host "2/9 Reading department..."
Invoke-Api GET "/api/Department/$($department.id)" | Out-Null

Write-Host "3/9 Creating alumni..."
$alumni = Invoke-Api POST "/api/Alumni" @{
    fullName = "Smoke Test Graduate"
    email = "smoke.$stamp@example.com"
    phone = "777000000"
    graduationYear = 2025
    departmentId = $department.id
}

Write-Host "4/9 Reading alumni..."
Invoke-Api GET "/api/Alumni/$($alumni.id)" | Out-Null

Write-Host "5/9 Creating event..."
$clubEvent = Invoke-Api POST "/api/Event" @{
    title = "Smoke Test Event"
    description = "Automated API smoke test"
    eventDate = (Get-Date).AddDays(7).ToString("yyyy-MM-ddTHH:mm:ss")
    alumniId = $alumni.id
}

Write-Host "6/9 Reading event..."
Invoke-Api GET "/api/Event/$($clubEvent.id)" | Out-Null

Write-Host "7/9 Deleting event..."
Invoke-RestMethod -Method DELETE -Uri "$BaseUrl/api/Event/$($clubEvent.id)" | Out-Null

Write-Host "8/9 Deleting alumni..."
Invoke-RestMethod -Method DELETE -Uri "$BaseUrl/api/Alumni/$($alumni.id)" | Out-Null

Write-Host "9/9 Deleting department..."
Invoke-RestMethod -Method DELETE -Uri "$BaseUrl/api/Department/$($department.id)" | Out-Null

Write-Host "PASS: Department -> Alumni -> Event CRUD smoke test completed successfully."
