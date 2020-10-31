' Created By, Mohammad Hamdy Ghanem, 
' Egypt, 2020

Class FlagGenerator

    Public Shared Function Generate(FlageName As String, Flags() As String) As String
        Dim sbTemplate As New Text.StringBuilder(My.Resources.FlagTemplate)

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