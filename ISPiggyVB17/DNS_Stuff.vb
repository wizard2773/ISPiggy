﻿Imports System.Net
Module DNS_Stuff
    '---------------------------------------------------------------------------------------
    ' Procedure : toDotCom
    ' Purpose   : replaces .org and .net extensions to .com
    ' How to Use: strVar = toDotCom (strVar)
    '---------------------------------------------------------------------------------------
    Public Function toDotCom(strToCom As String)
        Dim iLength As Integer
        Dim iDotLoc As Integer
        Dim iDomOnly As Integer

        iLength = Len(strToCom)             ' get overall domain length
        iDotLoc = InStr(strToCom, ".")      ' get dot location in domain
        iDomOnly = (iLength - (iLength - iDotLoc + 1)) ' get size of domain

        If iDomOnly >= 2 Then               ' domain has at least two chars
            ' save only domain into variable and add .com
            strToCom = Mid(strToCom, 1, iDomOnly) & ".com"
        End If
        toDotCom = strToCom
    End Function

    '---------------------------------------------------------------------------------------
    ' Procedure : domainMinusOne
    ' Purpose   : shortens domain name by one char at the end
    ' How to Use: strVar = domainMinusOne (strVar)
    '---------------------------------------------------------------------------------------
    Public Function domainMinusOne(strDomain As String)
        Dim iLength As Integer
        Dim iDotLoc As Integer
        Dim iDomOnly As Integer
        Dim sExtOnly As String

        iLength = Len(strDomain)          ' get overall domain length
        iDotLoc = InStr(strDomain, ".")   ' get dot location in domain
        iDomOnly = (iLength - (iLength - iDotLoc + 1)) ' get size of domain

        If iDomOnly >= 3 Then                       ' domain has at least two chars
            ' save extension into variable
            sExtOnly = Mid(strDomain, iDotLoc, iLength - iDotLoc + 1)
            strDomain = Mid(strDomain, 1, iDomOnly)  ' save only domain into variable

            ' remove last char in domain name and reassemble domain name
            domainMinusOne = Mid(strDomain, 1, iDotLoc - 2) & sExtOnly
            strDomain = domainMinusOne  ' puts new domain in passed domain variable
        End If
        domainMinusOne = strDomain
    End Function

    '---------------------------------------------------------------------------------------
    ' Procedure : domainMinusRand
    ' Purpose   : shortens domain name by one random char
    ' How to Use: strVar = domainMinusRand (strVar)
    '---------------------------------------------------------------------------------------
    Public Function domainMinusRand(strDomain As String)
        Dim iLength As Integer
        Dim iDotLoc As Integer
        Dim iDomOnly As Integer
        Dim iRndChar As Integer
        Dim sExtOnly As String
        Dim sRndChar As String

        iLength = Len(strDomain)          ' get overall domain length
        iDotLoc = InStr(strDomain, ".")   ' get dot location in domain
        iDomOnly = (iLength - (iLength - iDotLoc + 1)) ' get size of domain

        If iDomOnly >= 3 Then                       ' domain has at least two chars
            ' save extension into variable
            sExtOnly = Mid(strDomain, iDotLoc, iLength - iDotLoc + 1)
            strDomain = Mid(strDomain, 1, iDomOnly)  ' save only domain into variable

            iRndChar = myRandom(1, iDomOnly)        ' get random char int location from domain length
            sRndChar = Mid(strDomain, iRndChar, 1)  ' gets random char from domain

            ' remove char from domain and reassemble domain name
            domainMinusRand = Replace(strDomain, sRndChar, "", , 1) & sExtOnly
            strDomain = domainMinusRand         ' puts new domain in passed domain variable
        End If
        domainMinusRand = strDomain
    End Function

    '---------------------------------------------------------------------------------------
    ' Procedure : makeDomainName
    ' Purpose   : makes a random Domain Name
    ' How to Use: strVar = makeDomainName ()
    '---------------------------------------------------------------------------------------
    Public Function makeDomainName() As String
        Dim sRandWeb As String
        Dim iDomain As Integer
        Dim x As Integer

        Dim astrVowels() As String
        Dim astrConsonants() As String
        Dim astrTLDsuffix() As String

        astrVowels = {"a", "e", "i", "o", "u", "y"}
        astrConsonants = {"c", "d", "f", "h", "l", "m", "n", "r", "s", "t"}
        astrTLDsuffix = {".com", ".com", ".net", ".org"}

        sRandWeb = ""
        x = 0

        While (x < myRandom(iMinDomain, iMaxDomain))
            If (isOdd(x) = 1) Then      ' even number generates random vowel 
                sRandWeb = sRandWeb & astrVowels(myRandom(0, UBound(astrVowels)))
            Else                        ' odd number generates random consonant
                sRandWeb = sRandWeb & astrConsonants(myRandom(0, UBound(astrConsonants)))
            End If
            x = x + 1
        End While

        iDomain = myRandom(0, UBound(astrTLDsuffix)) ' get random domain suffix

        makeDomainName = sRandWeb & astrTLDsuffix(iDomain)
    End Function

    Public Function DoGetHostEntry(hostName As String)
        Form1.btnIndicator.BackColor = Color.Red    ' set visual wait indicator
        Form1.GetIP.Enabled = False     ' disable the button to avoid a double press
        Form1.stat01.Text = "WORKING HARD... PLEASE WAIT..."
        Form1.Refresh()                 ' paint the the shapes before next operation

        Try
            Dim host As IPHostEntry = Dns.GetHostEntry(hostName)

            Dim sMyString As String
            sMyString = ""

            Dim ip As IPAddress() = host.AddressList

            sMyString = ip(0).ToString()    ' get first IP returned from DNS server
            Form1.stat01.Text = sSuccess & sMyString & " Found!"
            GC.Collect()        ' garbage collection for unclaimed memory

        Catch ex As System.Net.Sockets.SocketException
            Form1.stat01.Text = ex.Message & "... trying next host..."
            GC.Collect()        ' garbage collection for unclaimed memory
        Catch ex As System.Exception
            Form1.stat01.Text = ex.Message
            GC.Collect()        ' garbage collection for unclaimed memory
        Finally
            GC.Collect()        ' garbage collection for unclaimed memory
        End Try
        GC.Collect()            ' garbage collection for unclaimed memory

        Form1.btnIndicator.BackColor = Color.Green  ' set visual indicator
        Form1.GetIP.Enabled = True

        DoGetHostEntry = Nothing
    End Function

End Module
