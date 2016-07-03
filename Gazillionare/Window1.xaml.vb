Imports System.ComponentModel

Public Class Window1
    Private strCurrentLocation As String = ""
    Private twr As New TravelWindowResults
    Public Property CurrentCity() As String
        Get
            Return strCurrentLocation
        End Get
        Set(value As String)
            strCurrentLocation = value
        End Set
    End Property
    Public Function GetTravelWindowResults() As TravelWindowResults
        Return twr
    End Function
    Public Sub TravelHandler(sender As Button, e As RoutedEventArgs) Handles trav1.Click, trav2.Click, trav3.Click, trav4.Click,
            trav5.Click, trav6.Click, trav7.Click, trav8.Click, trav9.Click, btnGoBack.Click

        If sender.Content = "Go Back" Then
            Me.Close()
            Exit Sub
        End If

        'Stuff here should change our current location, as well as change the marketplace.
        'Manipulate the 'TravelWindowResults' class to add properties you need to change marketplace and other
        twr.City = sender.Content
        twr.State = City2State(twr.City)
        Window1.Close()
    End Sub

    Private Sub gridTravel_Initialized(sender As Object, e As EventArgs) Handles gridTravel.Initialized
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
            If n.Content = Me.GetTravelWindowResults.City Then
                n.IsEnabled = False
            Else
                n.IsEnabled = True
            End If
        Next
    End Sub
    Public Function City2State(strCity As String) As String
        Select Case strCity
            Case "Springfield", "Nixa", "Republic"
                Return "MO"
            Case "Chantilly", "Reston", "Herndon"
                Return "VA"
            Case "Lowell", "Chelmsford", "Billerica"
                Return "MA"
            Case Else
                '
        End Select
    End Function
End Class
