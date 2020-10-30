' Created By, Mohammad Hamdy Ghanem, 
' Egypt, 2020

Class FlagGenerator

    Public Shared Function Generate(FlageName As String, Flags() As String) As String
        Dim sbTemplate As New Text.StringBuilder(
"
Class #FlagName#
#Flags#

    Public Shared ReadOnly NoneSet As #FlagName# = 0
    Public Shared ReadOnly AllSet As #FlagName# = #MaxValue#

    Public Shared ReadOnly Property Flags As #FlagName#() = {#FlagList#}
    Public Shared ReadOnly Property FlagNames As String() = {#StrFlagList#}
    Public Shared ReadOnly Property FlagValues As Integer() = {#ValueList#}
    Public ReadOnly Property Name As String

    Public ReadOnly Property OnFlags As List(Of #FlagName#)
        Get
            Dim lstFlags As New List(Of #FlagName#)
            For Each flag In Flags
                If (Value And flag.Value) > 0 Then lstFlags.Add(flag)
            Next
            Return lstFlags
        End Get
    End Property

    Public ReadOnly Property OffFlags As List(Of #FlagName#)
        Get
            Dim lstFlags As New List(Of #FlagName#)
            For Each flag In Flags
                If (Value And flag.Value) = 0 Then lstFlags.Add(flag)
            Next
            Return lstFlags
        End Get
    End Property


    Dim Value As Integer

    Private Sub New(value As Integer)
        Me.Value = value
    End Sub

    Private Sub New(name As String, value As Integer)
        _Name = name
        Me.Value = value
    End Sub

    Public Shared Widening Operator CType(value As Integer) As #FlagName#
        Return New #FlagName#(value)
    End Operator

    Public Shared Narrowing Operator CType(flag As #FlagName#) As Integer
        Return flag.Value
    End Operator

    Public Overrides Function ToString() As String
        Dim sb As New Text.StringBuilder
        For Each flag In Flags
            If (Value And flag.Value) > 0 Then
                If sb.Length > 0 Then sb.Append("" + "")
                sb.Append(flag.Name)
            End If
        Next
        Return sb.ToString
    End Function

    Public Function ToInteger() As Integer
        Return Value
    End Function

    Public Function SetFlags(ParamArray flags() As #FlagName#) As #FlagName#
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v = Value
        For Each flag In flags
            v = v Or flag.Value
        Next
        Return v
    End Function

    Public Function SetAllExcxept(ParamArray flags() As #FlagName#) As #FlagName#
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v As Integer = 0
        For Each flag In flags
            v = v Or flag.Value
        Next
        Return Value And Not v
    End Function


    Public Function UnsetFlags(ParamArray flags() As #FlagName#) As #FlagName#
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v = Value
        For Each flag In flags
            v = v And Not flag.Value
        Next
        Return v
    End Function

    Public Function UnsetAllExcxept(ParamArray flags() As #FlagName#) As #FlagName#
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v As Integer = 0
        For Each flag In flags
            v = v Or flag.Value
        Next
        Return v
    End Function

    Public Function ToggleFlags(ParamArray flags() As #FlagName#) As #FlagName#
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v = Value
        For Each flag In flags
            v = v Xor flag.Value
        Next
        Return v
    End Function

    Public Function ToggleAllFlags() As #FlagName#
        Return Value Xor #MaxValue#
    End Function

    Public Function AreAllSet(ParamArray flags() As #FlagName#) As Boolean
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value = #MaxValue#

        For Each flag In flags
            If (Value And flag.Value) = 0 Then Return False
        Next
        Return True
    End Function

    Public Function AreAllUnset(ParamArray flags() As #FlagName#) As Boolean
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value = 0

        For Each flag In flags
            If (Value And flag.Value) <> 0 Then Return False
        Next
        Return True
    End Function

    Public Function AreAnySet(ParamArray flags() As #FlagName#) As Boolean
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value > 0

        For Each flag In flags
            If (Value And flag.Value) <> 0 Then Return True
        Next
        Return False
    End Function

    Public Function AreAnyUnset(ParamArray flags() As #FlagName#) As Boolean
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value < #MaxValue#

        For Each flag In flags
            If (Value And flag.Value) = 0 Then Return True
        Next
        Return False
    End Function
End Class
")

        sbTemplate.Replace("#FlagName#", FlageName)

        Dim L = Flags.Length
        Dim MaxValue = 2 ^ L - 1
        sbTemplate.Replace("#MaxValue#", MaxValue)

        Dim sbFlags As New Text.StringBuilder
        Dim sbFlagList As New Text.StringBuilder
        Dim sbStrFlagList As New Text.StringBuilder
        Dim sbValueFlagList As New Text.StringBuilder

        For i = 0 To L - 1
            Dim flag = Chr(34) & Flags(i) & Chr(34)
            Dim value = 2 ^ i

            sbFlags.AppendLine($"    Public Shared ReadOnly {Flags(i)} As new {FlageName}({flag}, {value})")
            If sbFlagList.Length > 0 Then
                sbFlagList.Append(", ")
                sbStrFlagList.Append(", ")
                sbValueFlagList.Append(", ")
            End If
            sbFlagList.Append(Flags(i))
            sbStrFlagList.Append(flag)
            sbValueFlagList.Append(value)
        Next
        sbTemplate.Replace("#Flags#", sbFlags.ToString)
        sbTemplate.Replace("#FlagList#", sbFlagList.ToString)
        sbTemplate.Replace("#StrFlagList#", sbStrFlagList.ToString)
        sbTemplate.Replace("#ValueList#", sbValueFlagList.ToString)

        Return sbTemplate.ToString
    End Function



End Class