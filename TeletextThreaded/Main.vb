'//// Imports
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
'//// End Imports

Module Main

    'Dim subPage As Integer
    Dim multiPage As Boolean

    Public subPageData As New subPage()

    Class subPage
        Public subPageNumber As Integer = 1
        Public isSubPage As Boolean = False
        Public subPageCount As Integer = 0
        Public subPageWait As Integer = 0
    End Class

    Sub Main()
        '//// Main program.


        '//// Parse command line args.
        Dim Port As Integer = Nothing

        'If Command() <> Nothing Then '//// No command line arguments? Skip this.
        'Dim CLIArgs As Array
        'CLIArgs = Split(Command, " ") '//// Split command line args into an array.
        'For i = LBound(CLIArgs) To UBound(CLIArgs) '//// Check arguments.
        ' Select Case LCase(CLIArgs(i))
        '     Case "-p", "/p", "-port", "/port" '//// Argument: User wants a custom port number.
        ' If i = UBound(CLIArgs) Then
        ' '//// No value specified.
        ' '//// User forgot to add a port number.
        ' Report("No port specified, using standard port.") '//// DEBUG: Tell user they fucked up.
        ' Port = 5500 '//// Set the port for them.
        ' Else
        ' '//// User specified something.
        ' i = i + 1 '//// Check the next value.
        ' End If
        ' If Left(CLIArgs(i), 1) = "-" Or Left(CLIArgs(i), 1) = "/" Then
        ' '//// User forgot to add a port number.
        ' Report("Invalid port specified, using standard port.") '//// DEBUG: Tell user they fucked up.
        ' Port = 5500 '//// Set the port for them.
        ' Else
        ' If Int(Val(CLIArgs(i))) > 0 And Int(Val(CLIArgs(i))) < 65535 Then '//// Make sure user's port suggestion is valid.
        ' Port = CLIArgs(i) '//// If it is, set the port number.
        ' Else
        ' Port = 5500 '//// If it isn't, set the port for them.
        ' End If
        ' End If
        '     Case "-?", "/?", "-help", "/help", "-about", "/about" '//// Argument: User wants a custom port number.
        ' Report(vbCrLf & "TNXT-P 1.0 by Moonlit.")
        ' Report(vbCrLf & "Command line arguments:")
        ' Report("Help (this): -? or -help")
        ' Report("Custom port number: -p [port]")
        ' End
        ' End Select
        ' Next
        'Else
        Port = 5500 '//// If a custom port wasn't supplied, use the default one.
        'End If

        '//// Variables
        Dim serverSocket As New TcpListener(Port) '//// Depreciated method.
        Dim clientSocket As TcpClient
        Dim counter As Integer
        '//// End Variables

        serverSocket.Start() '//// Start listening.
        Console.Clear() '//// DEBUG: Clear the console.
        Report("TNXT 1.5 running on " & Environment.OSVersion.ToString & ".") '//// DEBUG: Tell user we're listening.
        Report("Listening on port " & Port & ". Ready.")
        counter = 0 '//// No clients.

        While (True) '//// Loop to wait for new clients.
            counter += 1 '//// Increase counter, we have a new client.
            clientSocket = serverSocket.AcceptTcpClient() '//// Accept the connection.
            Report("Client Connected: " & Convert.ToString(counter) & "") '//// DEBUG: Tell user we have a new client.
            Dim client As New handleClient '//// New instance of handleClient to deal with the new client.
            client.startClient(clientSocket, Convert.ToString(counter)) '//// Start client code.
            Thread.Sleep(50)
        End While

        clientSocket.Close() '//// Close client socket(s).
        serverSocket.Stop() '//// Stop thread(s).
        Report("Quitting.") '//// DEBUG: Tell user we're quitting.
        Console.ReadLine()
        '//// End of program.
    End Sub

    Sub Report(ByVal msg As String)
        '//// Send messages to the console.
        Console.WriteLine(msg) '//// DEBUG: Tell user whatever's in the msg variable.
    End Sub

    Public Class handleClient
        '//// Client thread code.

        '//// Variables
        Dim clientSocket As TcpClient
        Dim clNo As String
        '//// End Variables

        Public Sub startClient(ByVal inClientSocket As TcpClient, ByVal clineNo As String)
            '//// Handle new client.
            Me.clientSocket = inClientSocket
            Me.clNo = clineNo '//// Set client number.
            Dim ClientThread As Thread = New Thread(AddressOf ClientLoop) '//// Create new client thread. 
            ClientThread.Start() '//// Run new thread.
        End Sub

        Private Sub ClientLoop()
            '//// Main code for each client.

            '//// Variables
            Dim ReceiveData(127) As Byte
            Dim PageRequest As String = "100"
            Dim PageData As String
            Dim SendBytes As Byte()
            Dim serverResponse As String
            Dim networkStream As NetworkStream = clientSocket.GetStream()
            '//// End Variables

            Dim ClockThread As Thread = New Thread(AddressOf Update) '//// Create new clock update thread.
            ClockThread.Start() '//// Run new thread.

            serverResponse = AssemblePage(PageRequest) '//// Generate initial page.
            SendBytes = Encoding.Unicode.GetBytes(serverResponse)
            networkStream.Write(SendBytes, 0, SendBytes.Length) '//// Send page.
            networkStream.Flush() '/// Flush the send queue.

            While (True)
                Try
                    '//// Try the following:
                    'Report("")
                    networkStream.Read(ReceiveData, 0, 127) '//// Yay, new data.
                    PageRequest = System.Text.Encoding.Default.GetString(ReceiveData) '//// Get new page request.
                    PageRequest = Int(Val(Strings.Left(PageRequest, 3))) '//// Grab the numeric value of the first 3 characters.
                    networkStream.Flush()

                    If PageRequest >= 100 And PageRequest <= 999 Then '//// Page number too low, send page 100. Could be used to point to howto page.
                        Report("Page Requested: Client " & clNo & " requested page " & PageRequest & ".") '//// DEBUG: Tell user a page has been requested and by which client.
                        PageData = AssemblePage(PageRequest)
                        serverResponse = PageData '//// Generate requested page.
                        SendBytes = Encoding.Unicode.GetBytes(serverResponse)
                        networkStream.Write(SendBytes, 0, SendBytes.Length) '//// Send page.
                        networkStream.Flush() '//// Flush send queue.
                        PageRequest = Nothing
                    End If

                    '//// If something went wrong:
                Catch ex As Exception
                    '//// Something went wrong, the client probably quit.
                    Report("Client Disconnected: " & Me.clNo & ". Killing Page Thread.") '//// DEBUG: Tell user client has disconnected.
                    Exit Sub
                    Report("Client Disconnected: " & Me.clNo & ". End Page Thread.")
                    Thread.CurrentThread.Abort() '//// Abort the thread.
                    Report("Client Disconnected: " & Me.clNo & ". Killed Page Thread.")
                End Try
            End While

        End Sub

        Private Sub Update()
            '//// Page update thread. 
            '//// Variables
            Dim SendBytes As Byte()
            Dim serverResponse As String
            Dim networkStream As NetworkStream = clientSocket.GetStream()
            Dim pageData As String
            '//// End Variables
            While True
                Try
                    '//// Try the following:


                    '//// EXPERIMENTAL SUBPAGE CODE
                    SyncLock subPageData
                        If subPageData.isSubPage = True Then
                            If subPageData.subPageWait = 10 Then
                                subPageData.subPageWait = 0
                                If subPageData.subPageNumber > subPageData.subPageCount Then subPageData.subPageNumber = 1
                                If subPageData.subPageNumber <= 0 Then subPageData.subPageNumber = 1
                                'Report("Subpage Requested: Client " & clNo & " requested page " & subPageData.subPageNumber & ".") '//// DEBUG: Tell user a page has been requested and by which client.
                                pageData = AssemblePage(subPageData.subPageNumber)
                                serverResponse = pageData '//// Generate requested page.
                                SendBytes = Encoding.Unicode.GetBytes(serverResponse)
                                networkStream.Write(SendBytes, 0, SendBytes.Length) '//// Send page.
                                networkStream.Flush() '//// Flush send queue.
                                subPageData.subPageNumber = subPageData.subPageNumber + 1
                            End If
                            subPageData.subPageWait = subPageData.subPageWait + 1
                        End If
                    End SyncLock
                    '//// EXPERIMENTAL SUBPAGE CODE

                    serverResponse = RefreshClock() '//// Generate new clock data.
                    SendBytes = Encoding.Unicode.GetBytes(serverResponse)
                    networkStream.Write(SendBytes, 0, SendBytes.Length) '//// Send the new clock data.
                    networkStream.Flush() '//// Flush the send queue.
                    Thread.Sleep(1000) '//// Wait 1 second.

                    '//// If something went wrong:
                Catch ex As Exception
                    '//// Something went wrong, the client probably quit.
                    Report(ex.ToString)

                    Report("Client Disconnected: " & Me.clNo & ". Killing Clock Thread.") '//// DEBUG: Tell user client has disconnected.
                    Exit Sub
                    Report("Client Disconnected: " & Me.clNo & ". End Clock Thread.")
                    Thread.CurrentThread.Abort() '//// Abort the thread.
                    Report("Client Disconnected: " & Me.clNo & ". Killed Clock Thread.")
                End Try
            End While
        End Sub
    End Class
End Module
