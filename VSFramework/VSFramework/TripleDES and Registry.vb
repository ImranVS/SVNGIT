Imports System
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports Microsoft.Win32
Imports Microsoft.Win32.Registry
Imports System.Xml
Imports System.Xml.Linq
Imports System.Configuration

Imports MongoDB.Driver

'Updated 11-7-13
Public Class TripleDES
    Private key() As Byte = {8, 2, 11, 4, 5, 6, 7, 8, 4, 10, 11, 12, 13, 21, 15, 16, 17, 18, 2, 20, 21, 16, 16, 24} 'Encryption Key
    Private iv() As Byte = {65, 110, 68, 1, 69, 178, 200, 235} 'Initialization Vector

    Public Function Encrypt(ByVal plainText As String) As Byte()
        ' Declare a UTF8Encoding object so we may use the GetByte
        ' method to transform the plainText into a Byte array.
        Dim utf8encoder As UTF8Encoding = New UTF8Encoding
        Dim inputInBytes() As Byte = utf8encoder.GetBytes(plainText)

        ' Create a new TripleDES service provider
        Dim tdesProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider

        ' The ICryptTransform interface uses the TripleDES
        ' crypt provider along with encryption key and init vector
        ' information
        Dim cryptoTransform As ICryptoTransform = tdesProvider.CreateEncryptor(Me.key, Me.iv)

        ' All cryptographic functions need a stream to output the
        ' encrypted information. Here we declare a memory stream
        ' for this purpose.
        Dim encryptedStream As MemoryStream = New MemoryStream
        Dim cryptStream As CryptoStream = New CryptoStream(encryptedStream, cryptoTransform, CryptoStreamMode.Write)

        ' Write the encrypted information to the stream. Flush the information
        ' when done to ensure everything is out of the buffer.
        cryptStream.Write(inputInBytes, 0, inputInBytes.Length)
        cryptStream.FlushFinalBlock()
        encryptedStream.Position = 0

        ' Read the stream back into a Byte array and return it to the calling
        ' method.
        Dim result(encryptedStream.Length - 1) As Byte
        encryptedStream.Read(result, 0, encryptedStream.Length)
        cryptStream.Close()
        Return result
    End Function

    Public Function Decrypt(ByVal inputInBytes() As Byte) As String
        If inputInBytes Is Nothing Then Exit Function
        ' UTFEncoding is used to transform the decrypted Byte Array
        ' information back into a string.
        Dim utf8encoder As UTF8Encoding = New UTF8Encoding
        Dim tdesProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider

        ' As before we must provide the encryption/decryption key along with
        ' the init vector.
        Dim cryptoTransform As ICryptoTransform = tdesProvider.CreateDecryptor(Me.key, Me.iv)

        ' Provide a memory stream to decrypt information into
        Dim decryptedStream As MemoryStream = New MemoryStream
        Dim cryptStream As CryptoStream = New CryptoStream(decryptedStream, cryptoTransform, CryptoStreamMode.Write)
        cryptStream.Write(inputInBytes, 0, inputInBytes.Length)
        cryptStream.FlushFinalBlock()
        decryptedStream.Position = 0

        ' Read the memory stream and convert it back into a string
        Dim result(decryptedStream.Length - 1) As Byte
        decryptedStream.Read(result, 0, decryptedStream.Length)
        cryptStream.Close()
        Dim myutf As UTF8Encoding = New UTF8Encoding
        Return myutf.GetString(result)
    End Function
End Class

Public Class XMLOperation

    ''' <summary>
    ''' get number of nodes in a parent node
    ''' </summary>
    ''' <param name="strNode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNodeCount(ByVal strNode As String)
        Dim xml_doc As New System.Xml.XmlDocument
        'xml_doc.Load("Settings.xml")
        Dim path As String
        path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)

        ' xml_doc.Load("C:\Program Files (x86)\VitalSigns\Settings.xml")
        xml_doc.Load(path & "\Settings.xml")
        Dim strVal As String = ""
        Dim xmlnode As XmlNodeList

        xmlnode = xml_doc.GetElementsByTagName("Settings")
        Return xmlnode(0).ChildNodes.Count

    End Function

    ''' <summary>
    ''' Read Value or Data Type from XML based on Setting Name
    ''' </summary>
    ''' <param name="strSetting">Setting Name</param>
    ''' <param name="strNode">Value/Type</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReadFromXML(ByVal strSetting As String, ByVal strNode As String)
        Dim xml_doc As New System.Xml.XmlDocument, strType As String = "", strValue As Object
        Dim str1() As String

        Dim path As String
        Path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
        ' xml_doc.Load("C:\Program Files (x86)\VitalSigns\Settings.xml")
        xml_doc.Load(Path & "\Settings.xml")

        Dim strVal As String = ""
        Dim child_nodes As XmlNodeList = xml_doc.GetElementsByTagName("name")
        Dim parentnode As XmlNode
        For Each child As XmlNode In child_nodes
            If strSetting = child.InnerText Then
                parentnode = child.ParentNode
                If strNode = "Value" Then
                    If parentnode.ChildNodes(2).InnerText = "System.Byte[]" Then
                        strValue = parentnode.ChildNodes(1).InnerText
                        str1 = strValue.Split(",")
                        Dim bstr1(str1.Length - 1) As Byte
                        For j As Integer = 0 To str1.Length - 1
                            bstr1(j) = str1(j).ToString()
                        Next
                        Return bstr1
                    Else
                        Return parentnode.ChildNodes(1).InnerText
                    End If
                End If
                If strNode = "Type" Then
                    Return parentnode.ChildNodes(2).InnerText
                End If
            End If
        Next child
        Return ""
    End Function

    ''' <summary>
    ''' Write To XML file at the end- Setting name, value
    ''' If Byte Array object is sent, it will convert to comma seperated string
    ''' </summary>
    ''' <param name="strSetting">Name</param>
    ''' <param name="strValue">Value</param>
    ''' <remarks></remarks>
    Public Sub WriteToXML(ByVal strSetting As String, ByVal strValue As Object)
        Dim strChangeValue As String = "", strType As String = ""

        Dim xmlDoc As XDocument = XDocument.Load("Settings.xml")
        Dim iNodes As Integer = 0
        strType = Convert.ToString(strValue.GetType())
        If TypeOf strValue Is Byte() Then
            For i As Integer = 0 To strValue.Length - 1
                strChangeValue += strValue(i).ToString() + ","
            Next
            If strChangeValue.Length > 0 Then
                strChangeValue = strChangeValue.Substring(0, strChangeValue.Length - 1)
            End If
        Else
            strChangeValue = strValue
        End If
        If NodeExists(strSetting) = False Then
            iNodes = GetNodeCount("Settings")
            Dim attrAndValue As XElement = _
                <<%= "Setting" & Convert.ToString(iNodes + 1) %>>
                    <name><%= strSetting %></name>
                    <value><%= strChangeValue %></value>
                    <type><%= strType %></type>
                </>
            xmlDoc.Root.Add(attrAndValue)
            xmlDoc.Save("Settings.xml")
        Else
            ModifyNode(strSetting, "Value", strChangeValue)
            ModifyNode(strSetting, "Type", strType)
        End If
    End Sub

    ''' <summary>
    ''' Modify Setting - Name, Value, DataType
    ''' </summary>
    ''' <param name="strSetting">Setting Name</param>
    ''' <param name="strNode">Name/Value/Type</param>
    ''' <param name="strData">Data to be modified</param>
    ''' <remarks></remarks>
    Public Function ModifyNode(ByVal strSetting As String, ByVal strNode As String, ByVal strData As String)
        Dim xml_doc As New System.Xml.XmlDocument
        Dim path As String
        path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
        'xml_doc.Load("C:\Program Files (x86)\VitalSigns\Settings.xml")
        xml_doc.Load(path & "\Settings.xml")

        Dim strVal As String = ""
        Dim child_nodes As XmlNodeList = xml_doc.GetElementsByTagName("name")
        Dim parentnode As XmlNode
        For Each child As XmlNode In child_nodes
            If strSetting = child.InnerText Then
                parentnode = child.ParentNode
                If strNode = "Name" Then
                    parentnode.ChildNodes(0).InnerText = strData
                End If
                If strNode = "Value" Then
                    parentnode.ChildNodes(1).InnerText = strData
                End If
                xml_doc.Save("Settings.xml")
                Return True
            End If
        Next child
        Return False
    End Function

    ''' <summary>
    ''' Find if node exists
    ''' </summary>
    ''' <param name="strNodeValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NodeExists(ByVal strNodeValue As String)

        Dim xml_doc As New System.Xml.XmlDocument
        Dim path As String
        path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
        ' xml_doc.Load("C:\Program Files (x86)\VitalSigns\Settings.xml")
        xml_doc.Load(path & "\Settings.xml")

        Dim strVal As String = ""
        Dim child_nodes As XmlNodeList = xml_doc.GetElementsByTagName("name")
        For Each child As XmlNode In child_nodes
            If strNodeValue = child.InnerText Then
                Return True
            End If
        Next child
        Return False
    End Function


    ''' <param name="strChild"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function GetChildNodeValue(ByVal strNode As String, ByVal strChild As String)
        Dim MyXML As New XmlDocument()
        ' MyXML.Load("Settings.xml")

        Dim path As String
        path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
        MyXML.Load(path & "\Settings.xml")

        Dim MyXMLNode As XmlNode = MyXML.SelectSingleNode(strNode)

        If NodeExists(strNode) Then
            Dim MyXMLChildNode As XmlNode = MyXML.SelectSingleNode(strNode & "/" & strChild)
            Return MyXMLNode.ChildNodes(0).InnerText
        Else
            Return False
        End If
    End Function

    Public Sub CreateSQLDBSettings(ByVal vSQLServerName As String, ByVal vSQLIntegratedSecurity As String, ByVal vSQLDBName As String, ByVal vSQLUserName As String, ByVal vSQLPassword As String, ByVal vWorkstationName As String)

        WriteToXML("pSQLServerName", vSQLServerName)
        WriteToXML("pSQLIntegratedSecurity", vSQLIntegratedSecurity)
        ' WriteToXML("pSQLDBName", vSQLDBName)
        WriteToXML("pSQLUserName", vSQLUserName)
        WriteToXML("pSQLPassword", vSQLPassword)
        WriteToXML("pWorkstationName", vWorkstationName)

    End Sub

	Public Function GetDBConnectionString(ByVal SQLDatabaseName As String) As String
		'These two settings must be in machine.config's connectionstring section
		'<add name="VitalSignsConnectionString" connectionString="Data Source=localhost;Initial Catalog=VitalSigns;User ID=xx;Password=xx;Persist Security Info=True;multipleactiveresultsets=true; Min Pool Size=20;Max Pool Size=500; Connection Timeout=30;" providerName="System.Data.SqlClient"/>
		'<add name="VSS_StatisticsConnectionString" connectionString="Data Source=localhost;Initial Catalog=VSS_Statistics;User ID=xx;Password=xx;Persist Security Info=True;multipleactiveresultsets=true; Min Pool Size=20;Max Pool Size=500; Connection Timeout=30;" providerName="System.Data.SqlClient"/>

		Dim connectionStringName As String = ""
		If SQLDatabaseName = "" Then
			SQLDatabaseName = "VitalSigns"
		End If
		connectionStringName = SQLDatabaseName + "ConnectionString"
		Dim sConnectionString As String = ConfigurationManager.ConnectionStrings(connectionStringName).ToString()


		'Dim pSQLServerName As String, pSQLIntegratedSecurity As String, pSQLDBName As String, pSQLUserName As String, pSQLPassword As String, pWorkstationName As String
		'pSQLServerName = ReadFromXML("pSQLServerName", "Value")
		'pSQLIntegratedSecurity = ReadFromXML("pSQLIntegratedSecurity", "Value")
		''pSQLDBName = ReadFromXML("pSQLDBName", "Value")
		'pSQLDBName = SQLDatabaseName
		'pSQLUserName = ReadFromXML("pSQLUserName", "Value")
		'pSQLPassword = ReadFromXML("pSQLPassword", "Value")
		'pWorkstationName = ReadFromXML("pWorkstationName", "Value")

		'sConnectionString = "Data Source=" & pSQLServerName & "; Integrated Security=" & _
		'                   pSQLIntegratedSecurity & ";Initial Catalog=" & pSQLDBName & ";Persist Security Info=False;User ID=" & _
		'                   pSQLUserName & ";Password=" & pSQLPassword & ";Min Pool Size=20;Max Pool Size=500; Connection Timeout=30;"

		Return sConnectionString
	End Function

    Public Function GetMongoDBConnectionString() As String
        'These two settings must be in machine.config's connectionstring section
        '<add name="VitalSignsConnectionString" connectionString="Data Source=localhost;Initial Catalog=VitalSigns;User ID=xx;Password=xx;Persist Security Info=True;multipleactiveresultsets=true; Min Pool Size=20;Max Pool Size=500; Connection Timeout=30;" providerName="System.Data.SqlClient"/>
        '<add name="VSS_StatisticsConnectionString" connectionString="Data Source=localhost;Initial Catalog=VSS_Statistics;User ID=xx;Password=xx;Persist Security Info=True;multipleactiveresultsets=true; Min Pool Size=20;Max Pool Size=500; Connection Timeout=30;" providerName="System.Data.SqlClient"/>

        'Dim connectionStringName As String = ""
        'If SQLDatabaseName = "" Then
        '    SQLDatabaseName = "VitalSigns"
        'End If
        'connectionStringName = SQLDatabaseName + "ConnectionString"
        'Dim sConnectionString As String = ConfigurationManager.ConnectionStrings(connectionStringName).ToString()
        Dim sConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("VitalSignsMongo").ToString()

        'Dim pSQLServerName As String, pSQLIntegratedSecurity As String, pSQLDBName As String, pSQLUserName As String, pSQLPassword As String, pWorkstationName As String
        'pSQLServerName = ReadFromXML("pSQLServerName", "Value")
        'pSQLIntegratedSecurity = ReadFromXML("pSQLIntegratedSecurity", "Value")
        ''pSQLDBName = ReadFromXML("pSQLDBName", "Value")
        'pSQLDBName = SQLDatabaseName
        'pSQLUserName = ReadFromXML("pSQLUserName", "Value")
        'pSQLPassword = ReadFromXML("pSQLPassword", "Value")
        'pWorkstationName = ReadFromXML("pWorkstationName", "Value")

        'sConnectionString = "Data Source=" & pSQLServerName & "; Integrated Security=" & _
        '                   pSQLIntegratedSecurity & ";Initial Catalog=" & pSQLDBName & ";Persist Security Info=False;User ID=" & _
        '                   pSQLUserName & ";Password=" & pSQLPassword & ";Min Pool Size=20;Max Pool Size=500; Connection Timeout=30;"

        Return sConnectionString
    End Function


    Public Function ReadSettingsSQL(ByVal strSetting As String) As Object
        Dim returnVal As String

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.NameValue)(GetMongoDBConnectionString())
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.NameValue) = repository.Filter.Eq(Function(x) x.Name, strSetting)
            returnVal = repository.Find(filterDef).ToList()(0).Value.ToString()
        Catch ex As Exception
            returnVal = ""
        End Try

        Return returnVal

    End Function

    Public Sub WriteSettingsSQL(ByVal strSetting As String, ByVal strValue As Object)
        Dim vsobj As New VSAdaptor
        Dim strQuery As String, strUpdate As String, strInsert As String, strType As String, strChangeValue As String
        strChangeValue = ""
        strType = Convert.ToString(strValue.GetType())
        If TypeOf strValue Is Byte() Then
            For i As Integer = 0 To strValue.Length - 1
                strChangeValue += strValue(i).ToString() + ","
            Next
            If strChangeValue.Length > 0 Then
                strChangeValue = strChangeValue.Substring(0, strChangeValue.Length - 1)
            End If
        Else
            strChangeValue = strValue
        End If
        Dim sConnectionString As String = ""

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.NameValue)(GetMongoDBConnectionString())
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.NameValue) = repository.Filter.Eq(Function(x) x.Name, strSetting)
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.NameValue) = repository.Updater.Set(Function(x) x.Value, strChangeValue)
            repository.Upsert(filterDef, updateDef)
        Catch ex As Exception

        End Try

    End Sub
End Class


Public Class RegistryHandler
    ''' <summary>
    ''' WriteToRegistry Call from existing application
    ''' internally it will write to XML
    ''' </summary>
    ''' <param name="KeyName"></param>
    ''' <param name="KeyValue"></param>
    ''' <remarks></remarks>
    Sub WriteToRegistry(ByVal KeyName As String, ByVal KeyValue As Object)
        Dim objXML As New XMLOperation
        'objXML.WriteToXML(KeyName, KeyValue)
        objXML.WriteSettingsSQL(KeyName, KeyValue)
    End Sub


    ''' <summary>
    ''' Checks if registry has the keyname. 
    ''' If exists, takes the value, creates a setting in xml file, deletes registry key,returns value
    ''' If does not exist in registry, checks in settings.xml, if exists , returns value
    ''' </summary>
    ''' <param name="KeyName"></param>
    ''' <returns>value</returns>
    ''' <remarks></remarks>
    Function ReadFromRegistry(ByVal KeyName As String) As Object
        Dim objXML As New XMLOperation()
        Dim strValue As Object

        strValue = objXML.ReadSettingsSQL(KeyName)

        If strValue.ToString.Length > 0 Then
            Return strValue

        Else
            Dim aKey As RegistryKey
            ' aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Max Secure Anti Virus Plus")
            aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\MZLSoft")
            If aKey Is Nothing Then
                aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\MZLSoft")
            End If
            strValue = aKey.GetValue(KeyName)
            'Creates a setting in xml file
            'objXML.WriteToXML(KeyName, strValue)
            objXML.WriteSettingsSQL(KeyName, strValue)
            Try
                'Deletes Registry key
                'aKey.DeleteSubKey(KeyName)
            Catch ex As Exception
                Dim adapter As VSAdaptor
                adapter.WriteLogEntry(DateTime.Now.ToString + " Failed to read from Registry")
            End Try
            Return strValue
        End If
    End Function

    '8/6/2015 NS added for VSPLUS-2055
    Function ReadFromRegistry(ByVal KeyName As String, ByVal RetStr As Boolean) As Object
        Dim objXML As New XMLOperation()
        Dim strValue As Object
        Dim adapter As New VSAdaptor()

        strValue = objXML.ReadSettingsSQL(KeyName)

        If Not strValue Is Nothing Then
            If strValue.ToString.Length > 0 Then
                Return strValue
            Else
                Try
                    'Deletes Registry key
                    'aKey.DeleteSubKey(KeyName)
                    Dim aKey As RegistryKey
                    ' aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\MaxSecure Anti Virus Plus")
                    aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\MZLSoft")
                    If aKey Is Nothing Then
                        aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\MZLSoft")
                    End If
                    strValue = aKey.GetValue(KeyName)
                    'Creates a setting in xml file
                    'objXML.WriteToXML(KeyName, strValue)
                    If Not strValue Is Nothing Then
                        objXML.WriteSettingsSQL(KeyName, strValue)
                    End If
                Catch ex As Exception
                    adapter.WriteLogEntry(DateTime.Now.ToString + " Failed to read a value for " & KeyName & " from Registry - " & ex.Message, VSAdaptor.LogLevel.Verbose)
                    strValue = ""
                End Try
                Return strValue
            End If
        Else
            adapter.WriteLogEntry(DateTime.Now.ToString + " Could not find " & KeyName, VSAdaptor.LogLevel.Normal)
            Return strValue
        End If
	End Function

	Function ReadFromVitalSignsComputerRegistry(ByVal KeyName As String) As Object
		Dim objXML As New XMLOperation()
		Dim strValue As String
		Dim adapter As New VSAdaptor()

		Try

			strValue = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\VitalSigns", KeyName, "").ToString()

		Catch ex As Exception
			adapter.WriteLogEntry(DateTime.Now.ToString + " Failed to read a value for " & KeyName & " from Registry - " & ex.Message, VSAdaptor.LogLevel.Verbose)
			strValue = ""
		End Try

		Return strValue

	End Function

End Class

