Imports System.Text

Module Page

    '//// Assign variables.
    Dim CurrentPage As Integer
    Dim Body As String '//// Variable for page data.
    Dim FastText As String '//// Variable for FastText (coloured options) data.
    Dim FirstLine As String '//// Variable for header data. Contains clock and page.
    Dim FileNum As Integer '//// Variable for file access.

    '//// Easy names for colour/formatting data.
    Dim BGBlack As String = "[40m"
    Dim BGDarkRed As String = "[41m" '//// Not used?
    Dim BGDarkGreen As String = "[42m" '//// Not used?
    Dim BGDarkYellow As String = "[43m" '//// Not used?
    Dim BGDarkBlue As String = "[44m" '//// Not used?
    Dim BGDarkPurple As String = "[45m" '//// Not used?
    Dim BGDarkCyan As String = "[46m" '//// Not used?
    Dim BGDarkWhite As String = "[47m" '//// Not used?
    Dim BGReset As String = "[49m"
    Dim BGGrey As String = "[100m"
    Dim BGRed As String = "[101m"
    Dim BGGreen As String = "[102m"
    Dim BGYellow As String = "[103m"
    Dim BGBlue As String = "[104m"
    Dim BGPurple As String = "[105m"
    Dim BGCyan As String = "[106m"
    Dim BGWhite As String = "[107m"
    Dim FGBlack As String = "[30m"
    Dim FGDarkRed As String = "[31m" '//// Not used?
    Dim FGDarkGreen As String = "[32m" '//// Not used?
    Dim FGDarkYellow As String = "[33m" '//// Not used?
    Dim FGDarkBlue As String = "[34m" '//// Not used?
    Dim FGDarkPurple As String = "[35m" '//// Not used?
    Dim FGDarkCyan As String = "[36m" '//// Not used?
    Dim FGDarkWhite As String = "[37m" '//// Not used?
    Dim FGReset As String = "[39m"
    Dim FGGrey As String = "[90m"
    Dim FGRed As String = "[91m"
    Dim FGGreen As String = "[92m"
    Dim FGYellow As String = "[93m"
    Dim FGBlue As String = "[94m"
    Dim FGPurple As String = "[95m"
    Dim FGCyan As String = "[96m"
    Dim FGWhite As String = "[97m"
    '//// End of formatting data.

    Public Function AssemblePage(ByVal NextPage As Integer)

        If NextPage >= 100 Then CurrentPage = NextPage

        SyncLock subPageData
            If NextPage > 100 Then subPageData.subPageNumber = 1
        End SyncLock

        'Console.WriteLine("Generating page: " & CurrentPage) '//// DEBUG: Tell user page is being generated.

        '//// Generate header.
        FirstLine = BGReset & Chr(27) & "[1;1H" & Chr(27) & "[97;40m " & "P" & Format(CurrentPage, "000") & " " & Chr(27) & "[1;7H" & Chr(27) & "[22;30;107m" & "ChannelEM" & Chr(27) & "[97;40m" & " P" & Format(CurrentPage, "000") & " " & Format(Now, "ddd dd MMM") & Chr(27) & "[93m" & " " & Format(Now, "HH:mm/ss")
        '//// Generate FastText buttons.
        '//// FastTest disabled until compatibility fix. Replaced by version text temporarily. Fix this.
        'FastText = BGReset & Chr(27) & "[25;1H" & Chr(27) & "[91;40m" & "   TEST   " & Chr(27) & "[92;40m" & "   PAGE   " & Chr(27) & "[93;40m" & "   FAST   " & Chr(27) & "[94;40m" & "   TEXT   "
        FastText = BGReset & Chr(27) & "[97;40m" & CentreText("  TNXT Version 1.50 | 2012 | Moonlit", 25)

        '//// Start page generation.
        If NextPage = 999 Then '//// Check for reserved pages.
            '//// Catch page 887, send custom page.
            Body = Chr(27) & "[1;31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[0m[1;32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[0m[1;34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛÛ[34mÛ[31mÛ[32mÛ[34mÛ[31mÛ[0m"
        ElseIf NextPage = 888 Then
            '//// Catch page 888, send custom page.
            Body = FGRed & BGBlack
            Body = Body & CentreText("Reserved for subtitles.", 4) & vbCrLf
        Else '//// If page isn't reserved, send normal page.
            If My.Computer.FileSystem.DirectoryExists(CurrentPage) Then
                'Report("IS A SUBPAGE")
                SyncLock subPageData
                    subPageData.isSubPage = True
                    subPageData.subPageCount = System.IO.Directory.GetFiles(Environment.CurrentDirectory & "/" & CurrentPage).Length()
                    Body = readFile(Environment.CurrentDirectory & "/" & CurrentPage & "/" & subPageData.subPageNumber & ".ans")
                End SyncLock
            Else
                'Report("NOT A SUBPAGE")
                subPageData.isSubPage = False
                subPageData.subPageNumber = 0
                If NextPage < 100 Then
                    SyncLock subPageData
                        Report(Environment.CurrentDirectory & "/" & CurrentPage & "/" & subPageData.subPageNumber & ".ans")
                        Body = readFile(Environment.CurrentDirectory & "/" & CurrentPage & "/" & subPageData.subPageNumber & ".ans")
                    End SyncLock
                ElseIf My.Computer.FileSystem.FileExists(NextPage & ".ans") Then
                    Body = readFile(NextPage & ".ans")
                Else
                    Body = readFile("1000.ans")
                End If
            End If
        End If
        '//// End page generation.

        '//// Send page to main loop.
        If InStr(UCase(Environment.OSVersion.ToString), "WINDOWS") Then
            Return Chr(27) & "[2J" & Chr(27) & "[?25l" & FirstLine & vbCr & Body & FastText & Chr(27) & "[1;3H" & Chr(27) & "[0;1;32m"
        Else
            Return Chr(27) & "[2J" & Chr(27) & "[?25l" & FirstLine & vbLf & vbCr & Body & FastText & Chr(27) & "[1;3H" & Chr(27) & "[0;1;32m"
        End If

    End Function

    Public Function CentreText(ByVal Text As String, ByVal Line As Integer)
        '//// Add co-ords for centred text, send it back.
        Return "[" & Line & ";" & Int(20 - (Len(Text) / 2)) & "H" & Text
    End Function

    Public Function RefreshClock()
        '//// Refresh date/clock.
        'Return BGReset & Chr(27) & "[s" & Chr(27) & "[1;21H" & Chr(27) & "[97;40m " & Format(Now, "ddd dd MMM") & Chr(27) & "[93m" & " " & subPage & Chr(27) & "[u"
        Return BGReset & Chr(27) & "[s" & Chr(27) & "[1;21H" & Chr(27) & "[97;40m " & Format(Now, "ddd dd MMM") & Chr(27) & "[93m" & " " & Format(Now, "HH:mm/ss") & Chr(27) & "[u"
    End Function

    Public Function readFile(ByVal NextPage As String)

        '//// Check for page, if exists send page <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        '//// Method 1: Windows
        If InStr(UCase(Environment.OSVersion.ToString), "WINDOWS") Then
            'Report("Windows file reading routine...")
            FileNum = FreeFile()
            NextPage = Replace(NextPage, "\", "/")
            'Report(NextPage)
            FileOpen(FileNum, NextPage, OpenMode.Input, OpenAccess.Read)
            Body = ""
            Do Until EOF(FileNum)
                Body = Body & Environment.NewLine & LineInput(FileNum)
            Loop
            'If InStr(Body, Chr(10)) Then End
            FileClose(FileNum)
            Body = Replace(Body, Chr(10), Chr(10) & Chr(13))
        Else
            '//// Check for page, if exists send page <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            '//// Method 2: Unix
            'Report("*n?x file reading routine...")

            Dim fileContents() As Byte = IO.File.ReadAllBytes(NextPage)
            NextPage = Replace(NextPage, "\", "/")
            'Report(NextPage)
            Body = ""
            For x = 0 To fileContents.Length - 1
                'Report("Char: " & fileContents(x) & Environment.NewLine)
                If Chr(fileContents(x)) = Environment.NewLine Then
                    'Report("------------------E-NL FOUND")
                    Body = Body & Environment.NewLine & vbCr
                Else
                    Body = Body & Chr(fileContents(x))
                End If
            Next
            'Report("Page found...")
        End If

        Return Body
    End Function
End Module
