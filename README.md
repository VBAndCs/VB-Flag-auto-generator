# Flag auto-generator
![Flags](https://user-images.githubusercontent.com/48354902/97733916-1c518380-1ae1-11eb-8454-c197d006b826.jpg)
A Windows Forms app, that allows you to enter the name of the Flag and its members, then press the Generate button to auto-generate a class that represents this flags and its members, and many methods to do flag operations and set and unset flags.
The underline type of the flag is UInteger, and the class contains CType, Logicak and arithmetic operators to convert between them. 
This is a list of the members I added to the class, noting that the flag is immutable, and methods don't change its state, but return a new instance carring the result:
```
Shared Fields:  
    None
    All
Shared Properties:
    Flags
    FlagNames 
    FlagValues
Instance Properties:
    Name
    OnFlags 
    OffFlags
    IsSet(flag)  ' A default readonly property (Indexer) that returns true if the given flag is set.
Instance Methods:
    ToString()
    ToString(separator)
    ToInteger
    SetFlag
    SetFlags
    SetAllExcxept
    UnsetFlag
    UnsetFlags
    UnsetAllExcxept
    ToggleFlag
    ToggleFlags
    ToggleAllFlags()
    AreAllSet
    AreAllUnset
    AreAnySet
    AreAnyUnset
```

And this is a sample of an auto-generated Flag:
```VB.NET
Class MyFlag
    Public Shared ReadOnly X As New MyFlag("X", 1)
    Public Shared ReadOnly Y As New MyFlag("Y", 2)
    Public Shared ReadOnly Z As New MyFlag("Z", 4)

    Const MaxValue As UInteger = 7
    Public Shared ReadOnly None As New MyFlag("None", 0)
    Public Shared ReadOnly All As New MyFlag("All", MaxValue)

    Private ReadOnly Value As UInteger

    Private Sub New(value As UInteger)
        Me.Value = If(value > MaxValue, MaxValue, value)
    End Sub

    Private Sub New(name As String, value As UInteger)
        _Name = name
        Me.Value = If(value > MaxValue, MaxValue, value)
    End Sub

    Public Shared ReadOnly Property Flags As MyFlag() = {X, Y, Z}
    Public Shared ReadOnly Property FlagNames As String() = {"X", "Y", "Z"}
    Public Shared ReadOnly Property FlagValues As UInteger() = {1, 2, 4}
    Public ReadOnly Property Name As String

    Public Property OnFlags As List(Of MyFlag)
        Get
            Dim lstFlags As New List(Of MyFlag)
            For Each flag In Flags
                If (Value And flag.Value) > 0 Then lstFlags.Add(flag)
            Next
            Return lstFlags
        End Get
        Set()
            SetFlags(Value.ToArray())
        End Set
    End Property

    Public Property OffFlags As List(Of MyFlag)
        Get
            Dim lstFlags As New List(Of MyFlag)
            For Each flag In Flags
                If (Value And flag.Value) = 0 Then lstFlags.Add(flag)
            Next
            Return lstFlags
        End Get
        Set()
            UnsetFlags(Value.ToArray)
        End Set
    End Property

    Default Public ReadOnly Property IsSet(flag As MyFlag) As Boolean
        Get
            Return (Value And flag.Value) > 0
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return ToString("+")
    End Function

    Public Overloads Function ToString(Separator As String) As String
        If Value = 0 Then Return None.Name
        If Value = MaxValue Then Return All.Name
        Dim sb As New Text.StringBuilder
        For Each flag In Flags
            If (Value And flag.Value) > 0 Then
                If sb.Length > 0 Then sb.Append(Separator)
                sb.Append(flag.Name)
            End If
        Next
        Return sb.ToString
    End Function

    Public Function ToInteger() As UInteger
        Return Value
    End Function

    Public Function SetFlags(ParamArray flags() As MyFlag) As MyFlag
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v = Value
        For Each flag In flags
            v = v Or flag.Value
        Next
        Return New MyFlag(v)
    End Function

    Public Function SetAllExcxept(ParamArray flags() As MyFlag) As MyFlag
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v As UInteger = 0
        For Each flag In flags
            v = v Or flag.Value
        Next
        Return New MyFlag(Value And Not v)
    End Function


    Public Function UnsetFlags(ParamArray flags() As MyFlag) As MyFlag
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v = Value
        For Each flag In flags
            v = v And Not flag.Value
        Next
        Return New MyFlag(v)
    End Function

    Public Function UnsetAllExcxept(ParamArray flags() As MyFlag) As MyFlag
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v As UInteger = 0
        For Each flag In flags
            v = v Or flag.Value
        Next
        Return New MyFlag(v)
    End Function

    Public Function ToggleFlags(ParamArray flags() As MyFlag) As MyFlag
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value

        Dim v = Value
        For Each flag In flags
            v = v Xor flag.Value
        Next
        Return New MyFlag(v)
    End Function

    Public Function ToggleAllFlags() As MyFlag
        Return New MyFlag(Value Xor 7)
    End Function

    Public Function AreAllSet(ParamArray flags() As MyFlag) As Boolean
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value = 7

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
        If flags Is Nothing OrElse flags.Length = 0 Then Return Value < 7

        For Each flag In flags
            If (Value And flag.Value) = 0 Then Return True
        Next
        Return False
    End Function
#Region "Operators"
    ' Contains all arithmetic and logical operators to work on Flags and Integers
    ' No need to publish them all here
#End Region
End Class
```

# To Do:
- Validate user inputs.
- Write tests for the FlagGenerator class.
- Crate a VS.NET extension to add a command to show the auto-generation window and add the generated file to the project.