$EXservers=Get-ExchangeServer
foreach($server in $EXservers){$exchCerts=Get-ExchangeCertificate| fl CertificateDomains, IsSelfSigned, Issuer, NotAfter, RootCAType, Subject, Thumbprint, Services}
$exchCerts 