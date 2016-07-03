Public Class Marketplace
    Private Shared arrCurrentData As List(Of Commodity)
    Private Shared arrStartingData As List(Of Commodity)
    Private Shared arrMyCargo As List(Of Commodity)

    Public Shared Property CurrentData As List(Of Commodity)
        Get
            Return arrCurrentData
        End Get
        Set(value As List(Of Commodity))
            arrCurrentData = value
        End Set
    End Property
    Public Shared Property StartingData As List(Of Commodity)
        Get
            Return arrStartingData
        End Get
        Set(value As List(Of Commodity))
            arrStartingData = value
        End Set
    End Property
    Public Sub Init()
        Dim rand As New Random
        Dim dt As GazDataDataSet.tblMarketItemsDataTable = New GazDataDataSetTableAdapters.tblMarketItemsTableAdapter().GetData()
        Dim rows() As GazDataDataSet.tblMarketItemsRow = dt.Select()
        Dim arrTemp As Array, arrStartingData As New List(Of Commodity)
        Dim comTemp As New Commodity

        arrTemp = rows.ToArray()
        For Each n In arrTemp
            comTemp = New Commodity
            comTemp.Name = n.fcommodity
            comTemp.MaxPrice = n.fmaxprice
            comTemp.MinPrice = n.fminprice
            comTemp.State = n.fState
            arrStartingData.Add(comTemp)
        Next

        StartingData = arrStartingData
    End Sub
    Public Function GetCurrentMarketplace(strState As String, Optional boolChangePrices As Boolean = True) As List(Of Commodity)
        Dim r As New Random
        Dim All As String = "All"
        Dim output As New List(Of Commodity)
        Dim x As New Commodity

        If boolChangePrices = False Then
            For Each n In CurrentData
                output.Add(n)
            Next
            Return output
            Exit Function
        End If
        'Everything specific to that state
        For Each n In StartingData
            n.Price = r.Next(n.MinPrice, n.MaxPrice)
            If n.State = strState Then
                output.Add(n)
            End If
        Next

        'and one random generic place
        x = StartingData(r.Next(0, StartingData.Count))
        Do Until x.State = "All"
            x = StartingData(r.Next(0, StartingData.Count))
        Loop
        output.Add(x)

        Return output
    End Function
    Public Function GetOwnedCommodities() As List(Of Commodity)
        Dim output As New List(Of Commodity)
        Dim n As New Commodity

        For Each n In CurrentData
            If n.QuantityOwned > 0 Then
                output.Add(n)
            End If
        Next

        Return output
    End Function
    Public Function Buy(lvMarketplace As ListView) As Decimal
        Dim decTotalTransactionPrice As Decimal, lngTotalTransactionCargo As Long, lngTotalCurrentnCargo As Long

        'Things to Check: Enough Money, Enough Cargo Area
        For Each n In CurrentData
            If n.QuantityOwned > 0 Then
                lngTotalCurrentnCargo += n.QuantityOwned
            End If
            If n.QuantityBuying > 0 Then
                decTotalTransactionPrice += n.QuantityBuying * n.Price
                lngTotalTransactionCargo += n.QuantityBuying
            End If
        Next

        If MainWindow.MarketResultsForMainWindow.Money < decTotalTransactionPrice Then

        End If


        'Once all checks are done, do this.
        For Each n In CurrentData
            If n.QuantityBuying > 0 Then
                n.QuantityOwned += n.QuantityBuying
                n.QuantityBuying = 0
            End If
        Next
        lvMarketplace.ItemsSource = Nothing
        lvMarketplace.ItemsSource = Me.CurrentData
        MainWindow.MarketResultsForMainWindow.Money -= decTotalTransactionPrice
        MainWindow.MarketResultsForMainWindow.TotalCargo += lngTotalTransactionCargo
        MainWindow.MarketResultsForMainWindow.Status = "All Good"
        'And return how much it cost
        Return lngTotalCurrentnCargo
    End Function
End Class

Public Class Commodity
    Private strName As String
    Private curPrice As Decimal
    Private curMinPrice As Decimal
    Private curMaxPrice As Decimal
    Private strState As String
    Private lngQuantityBuying As Long
    Private lngQuantitySelling As Long
    Private lngQuantityOwned As Long
    Public Property Name As String
        Get
            Return strName
        End Get
        Set(value As String)
            strName = value
        End Set
    End Property
    Public Property Price As Decimal
        Get
            Return curPrice
        End Get
        Set(value As Decimal)
            curPrice = value
        End Set
    End Property
    Public Property MaxPrice As Decimal
        Get
            Return curMaxPrice
        End Get
        Set(value As Decimal)
            curMaxPrice = value
        End Set
    End Property
    Public Property MinPrice As Decimal
        Get
            Return curMinPrice
        End Get
        Set(value As Decimal)
            curMinPrice = value
        End Set
    End Property
    Public Property State As String
        Get
            Return strState
        End Get
        Set(value As String)
            strState = value
        End Set
    End Property
    Public Property QuantityBuying As Long
        Get
            Return lngQuantityBuying
        End Get
        Set(value As Long)
            lngQuantityBuying = value
        End Set
    End Property

    Public Property QuantitySelling As Long
        Get
            Return lngQuantitySelling
        End Get
        Set(value As Long)
            lngQuantitySelling = value
        End Set
    End Property

    Public Property QuantityOwned As Long
        Get
            Return lngQuantityOwned
        End Get
        Set(value As Long)
            lngQuantityOwned = value
        End Set
    End Property
End Class
'SpecialtyOf