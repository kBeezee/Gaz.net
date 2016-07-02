Imports System.ComponentModel

Public Class Window1
    Private Shared strCurrentLocation As String = ""
    Public Shared Property CurrentLocation() As String
        Get
            Return strCurrentLocation
        End Get
        Set(value As String)
            strCurrentLocation = value
        End Set
    End Property

    Public Sub TravelHandler(sender As Button, e As RoutedEventArgs) Handles trav1.Click, trav2.Click, trav3.Click, trav4.Click,
            trav5.Click, trav6.Click, trav7.Click, trav8.Click, trav9.Click, btnGoBack.Click

        If sender.Content = "Go Back" Then
            Window1.Close()
        End If

        'Stuff here should change our current location, as well as change the marketplace.
        'Manipulate the 'TravelWindowResults' class to add properties you need to change marketplace and other
        MainWindow.TravelWindowResults.City = sender.Content
        Select Case sender.Content
            Case "Springfield", "Nixa", "Republic"
                MainWindow.TravelWindowResults.State = "MO"
            Case "Chantilly", "Reston", "Herndon"
                MainWindow.TravelWindowResults.State = "VA"
            Case "Lowell", "Chelmsford", "Billerica"
                MainWindow.TravelWindowResults.State = "MA"
        End Select
        Window1.Close()
    End Sub

    Private Sub gridTravel_Initialized(sender As Object, e As EventArgs) Handles gridTravel.Initialized
        Dim i As Long
        Dim NinePlaces As New Dictionary(Of String, String)

        NinePlaces.Add("trav1", "Springfield")
        NinePlaces.Add("trav2", "Nixa")
        NinePlaces.Add("trav3", "Republic")
        '
        NinePlaces.Add("trav4", "Chantilly")
        NinePlaces.Add("trav5", "Reston")
        NinePlaces.Add("trav6", "Herndon")
        '
        NinePlaces.Add("trav7", "Chelmsford")
        NinePlaces.Add("trav8", "Lowell")
        NinePlaces.Add("trav9", "Billerica")

        For Each n As Button In gridTravel.Children
            If NinePlaces.ContainsKey(n.Name) Then
                n.Content = NinePlaces.Item(n.Name)
            End If
        Next
    End Sub
    Private Sub Window1_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        For Each n As Button In gridTravel.Children
            If n.Content = Window1.CurrentLocation Then
                n.IsEnabled = False
            Else
                n.IsEnabled = True
            End If
        Next
    End Sub
End Class
