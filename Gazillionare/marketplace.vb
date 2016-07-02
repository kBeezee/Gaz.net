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
    Public Shared Property Cargo As List(Of Commodity)
        Get
            Return arrMyCargo
        End Get
        Set(value As List(Of Commodity))
            arrMyCargo = value
        End Set
    End Property
    Public Sub RefreshCurrentPrices(strState As String)
        Dim r As New Random
        Dim All As String = "All"
        Dim output As New List(Of Commodity)
        Dim x As New Commodity

        'Everything specific to that state
        For Each n In Me.StartingData
            n.Price = r.Next(n.MinPrice, n.MaxPrice)
            If n.State = strState Then
                output.Add(n)
            End If
        Next

        'and one random generic place
        x = Me.StartingData(r.Next(0, Me.StartingData.Count))
        Do Until x.State = "All"
            x = Me.StartingData(r.Next(0, Me.StartingData.Count))
        Loop
        output.Add(x)

        'effect changes to the property.
        Me.CurrentData = output
    End Sub
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
        CurrentData = arrStartingData
    End Sub
End Class

Public Class Commodity
    Private strName As String
    Private curPrice As Decimal
    Private curMinPrice As Decimal
    Private curMaxPrice As Decimal
    Private strState As String
    Private btnTestButton
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
End Class
'SpecialtyOf