# Flag auto-generator
![Flags](https://user-images.githubusercontent.com/48354902/97733916-1c518380-1ae1-11eb-8454-c197d006b826.jpg)
A Windows Forms app, that allows you to enter the name of the Flag and its members, then press the Generate button to auto-generate a class that represents this flags and its members, and many methods to do flag operations and set and unset flags.
The underline type of the flag is Integer, and the class contains CType operators to convert between them. This is a list of the members I added to the class:
Shared Fields:  
    NoneSet
    AllSet
Shared Properties:
    Flags
    FlagNames 
    FlagValues
Instance Properties:
    Name
    OnFlags 
    OffFlags
Instance Methods:
    ToString()
    ToString(separator)
    ToInteger
    SetFlags
    SetAllExcxept
    UnsetFlags
    UnsetAllExcxept
    ToggleFlags
    ToggleAllFlags()
    AreAllSet
    AreAllUnset
    AreAnySet
    AreAnyUnset

And this is a sample of an auto-generated Flag:
```VB.NET
Class MyFlag
    Public Shared ReadOnly X As new MyFlag("X", 1)
    Public Shared ReadOnly Y As new MyFlag("Y", 2)
    Public Shared ReadOnly Z As new MyFlag("Z", 4)
    Public Shared ReadOnly W As new MyFlag("W", 8)

    Public Shared ReadOnly NoneSet As New MyFlag ("None", 0)
    Public Shared ReadOnly AllSet As New MyFlag("All", 15)

    Public Shared ReadOnly Property Flags As MyFlag() = {X, Y, Z, W}
    Public Shared ReadOnly Property FlagNames As String() = {"X", "Y", "Z", "W"}
    Public Shared ReadOnly Property FlagValues As Integer() = {1, 2, 4, 8}
    Public ReadOnly Property Name As String

    Public ReadOnly Property OnFlags As List(Of MyFlag)
        Get
            Dim lstFlags As New List(Of MyFlag)
            For Each flag In Flags
                If (Value And flag.Value) > 0 Then lstFlags.Add(flag)
            Next
            Return lstFlags
        End Get
    End Property

    Public ReadOnly Property OffFlags As List(Of MyFlag)
        Get
            Dim lstFlags As New List(Of MyFlag)
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

    Public Shared Widening Operator CType(value As Integer) As MyFlag
        Return New MyFlag(value)
    End Operator

    Public Shared Narrowing Operator CType(flag As MyFlag) As Integer
        Return flag.Value
    End Operator


    Public Shared Operator +(flag As MyFlag, value As Integer) As Integer
        Return flag.Value + value
    End Operator

    Public Shared Operator -(flag As MyFlag, value As Integer) As Integer
        Return flag.Value - value
    End Operator

    Public Shared Operator *(flag As MyFlag, value As Integer) As Integer
        Return flag.Value * value
    End Operator

    Public Shared Operator /(flag As MyFlag, value As Integer) As Integer
        Return flag.Value / value
    End Operator

    Public Shared Operator ^(flag As MyFlag, value As Integer) As Long
        Return flag.Value ^ value
    End Operator
    
    Public Shared Operator Or(flag As MyFlag, value As Integer) As MyFlag
        Return New MyFlag(flag.Value Or value)
    End Operator

    Public Shared Operator And(flag As MyFlag, value As Integer) As MyFlag
        Return New MyFlag(flag.Value And value)
    End Operator

    Public Shared Operator Xor(flag As MyFlag, value As Integer) As MyFlag
        Return New MyFlag(flag.Value Xor value)
    End Operator

    Public Shared Operator Not(flag As MyFlag) As MyFlag
        Return New MyFlag(Not flag.Value)
    End Operator

    Public Shared Operator IsTrue(flag As MyFlag) As Boolean
        Return flag.Value > 0
    End Operator

    Public Shared Operator IsFalse(flag As MyFlag) As Boolean
        Return flag.Value = 0
    End Operator

    Public Shared Operator =(flag As MyFlag, value As Integer) As Boolean
        Return flag.Value = value
    End Operator

    Public Shared Operator <>(flag As MyFlag, value As Integer) As Boolean
        Return flag.Value <> value
    End Operator

    Public Shared Operator >(flag As MyFlag, value As Integer) As Boolean
        Return flag.Value > value
    End Operator

    Public Shared Operator <(flag As MyFlag, value As Integer) As Boolean
        Return flag.Value < value
    End Operator

    Public Shared Operator >=(flag As MyFlag, value As Integer) As Boolean
        Return flag.Value >= value
    End Operator

    Public Shared Operator <=(flag As MyFlag, value As Integer) As Boolean
        Return flag.Value <= value
    End Operator

    Public Overrides Function ToString() As String
        Return ToString("+")
    End Function

    Public Overloads Function ToString(Separator As String) As String
        If Value = 0 Then Return NoneSet.Name
        If Value = AllSet.Value Then Return AllSet.Name
        Dim sb As New Text.StringBuilder
        For Each flag In Flags
            If (Value And flag.Value) > 0 Then
                If sb.Length > 0 Then sb.Append(Separator)
                sb.Append(flag.Name)
            End If
        Next
        Return sb.ToString
    End Function

    Public Function ToInteger() As Integer
        Return Value
    End Function

    Public Function SetFlags(ParamArray flags() As MyFlag) As MyFlag
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v = Value
        For Each flag In flags
            v = v Or flag.Value
        Next
        Return v
    End Function

    Public Function SetAllExcxept(ParamArray flags() As MyFlag) As MyFlag
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v As Integer = 0
        For Each flag In flags
            v = v Or flag.Value
        Next
        Return Value And Not v
    End Function


    Public Function UnsetFlags(ParamArray flags() As MyFlag) As MyFlag
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v = Value
        For Each flag In flags
            v = v And Not flag.Value
        Next
        Return v
    End Function

    Public Function UnsetAllExcxept(ParamArray flags() As MyFlag) As MyFlag
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v As Integer = 0
        For Each flag In flags
            v = v Or flag.Value
        Next
        Return v
    End Function

    Public Function ToggleFlags(ParamArray flags() As MyFlag) As MyFlag
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v = Value
        For Each flag In flags
            v = v Xor flag.Value
        Next
        Return v
    End Function

    Public Function ToggleAllFlags() As MyFlag
        Return Value Xor 15
    End Function

    Public Function AreAllSet(ParamArray flags() As MyFlag) As Boolean
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value = 15

        For Each flag In flags
            If (Value And flag.Value) = 0 Then Return False
        Next
        Return True
    End Function

    Public Function AreAllUnset(ParamArray flags() As MyFlag) As Boolean
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value = 0

        For Each flag In flags
            If (Value And flag.Value) <> 0 Then Return False
        Next
        Return True
    End Function

    Public Function AreAnySet(ParamArray flags() As MyFlag) As Boolean
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value > 0

        For Each flag In flags
            If (Value And flag.Value) <> 0 Then Return True
        Next
        Return False
    End Function

    Public Function AreAnyUnset(ParamArray flags() As MyFlag) As Boolean
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value < 15

        For Each flag In flags
            If (Value And flag.Value) = 0 Then Return True
        Next
        Return False
    End Function
End Class
```

# To Do:
- Validate user inputs.
- Write tests for the FlagGenerator class.
- Crate a VS.NET extension to add a command to show the auto-generation window and add the generated file to the project.