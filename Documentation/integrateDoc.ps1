param(    
    [Parameter(Mandatory)]   
    [ValidateNotNullOrEmpty()] 
    [string]$commitArg 
)

& docfx docfx.json --warningsAsErrors
& git add .
& git commit -m `"$commitArg`" 
