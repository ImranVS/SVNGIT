#$server = 'jnittech-exchg1'
$ping = Test-Connection $server -Count 5 
$ping | Measure-Object ResponseTime -Average