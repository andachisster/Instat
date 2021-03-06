﻿' Instat-R
' Copyright (C) 2015
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License k
' along with this program.  If not, see <http://www.gnu.org/licenses/>.

Imports instat.Translations
Public Class dlgThreeVariableFrequencies
    Private bFirstLoad As Boolean = True
    Private bReset As Boolean = True
    Private bResetSubdialog As Boolean = False
    Private clsSjTab, clsSelect, clsSjPlot, clsGroupBy As New RFunction
    Private clsTableBaseOperator, clsGraphBaseOperator As New ROperator

    Private Sub dlgThreeVariableFrequencies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        autoTranslate(Me)
        If bFirstLoad Then
            InitialiseDialog()
            bFirstLoad = False
        End If
        If bReset Then
            SetDefaults()
        End If
        SetRCodeForControls(bReset)
        bReset = False
        TestOkEnabled()
    End Sub

    Private Sub SetRCodeForControls(bReset As Boolean)
        Dim clsTempParamOne As RParameter

        clsTempParamOne = New RParameter("x", 3)
        ucrReceiverGroups.AddAdditionalCodeParameterPair(clsSelect, clsTempParamOne, iAdditionalPairNo:=1)
        clsTempParamOne.bIncludeArgumentName = False

        ucrSelectorThreeVariableFrequencies.AddAdditionalCodeParameterPair(clsGraphBaseOperator, ucrSelectorThreeVariableFrequencies.GetParameter, iAdditionalPairNo:=1)

        ucrChkFlip.AddAdditionalCodeParameterPair(clsSjPlot, New RParameter("coord.flip", 1), iAdditionalPairNo:=1)
        ucrChkWeights.AddAdditionalCodeParameterPair(clsSjPlot, New RParameter("weight.by", 1), iAdditionalPairNo:=1)
        ucrReceiverWeights.AddAdditionalCodeParameterPair(clsSjPlot, ucrChkWeights.GetParameter(), iAdditionalPairNo:=1)

        ucrReceiverRowFactor.SetRCode(clsSelect, bReset)
        ucrReceiverColumnFactor.SetRCode(clsSelect, bReset)
        ucrReceiverGroups.SetRCode(clsGroupBy, bReset)
        ucrReceiverWeights.SetRCode(clsSjTab, bReset)
        ucrChkWeights.SetRCode(clsSjTab, bReset)
        ucrChkFlip.SetRCode(clsSjTab, bReset)
        ucrPnlFreqType.SetRCode(clsSjPlot, bReset)
        ucrPnlFrequencyDisplay.SetRCode(clsSjTab, bReset)
        ucrSelectorThreeVariableFrequencies.SetRCode(clsTableBaseOperator, bReset)
        ucrChkCell.SetRCode(clsSjTab, bReset)
        ucrChkColumn.SetRCode(clsSjTab, bReset)
        ucrChkRow.SetRCode(clsSjTab, bReset)
        ucrChkCount.SetRCode(clsSjTab, bReset)

    End Sub

    Private Sub InitialiseDialog()
        ucrReceiverColumnFactor.Selector = ucrSelectorThreeVariableFrequencies
        ucrReceiverWeights.Selector = ucrSelectorThreeVariableFrequencies
        ucrReceiverGroups.Selector = ucrSelectorThreeVariableFrequencies
        ucrReceiverRowFactor.SetDataType("factor")
        ucrReceiverColumnFactor.SetDataType("factor")
        ucrReceiverGroups.SetDataType("factor")
        ucrReceiverWeights.SetDataType("numeric")

        ucrSelectorThreeVariableFrequencies.SetParameter(New RParameter("data", 0))
        ucrSelectorThreeVariableFrequencies.SetParameterIsrfunction()

        ucrReceiverRowFactor.Selector = ucrSelectorThreeVariableFrequencies
        ucrReceiverRowFactor.SetParameter(New RParameter("var.row", 1))
        ucrReceiverRowFactor.SetParameterIsString()
        ucrReceiverRowFactor.bWithQuotes = False
        ucrReceiverRowFactor.SetParameterIncludeArgumentName(False)

        ucrReceiverColumnFactor.SetParameter(New RParameter("var.col", 2))
        ucrReceiverColumnFactor.SetParameterIsString()
        ucrReceiverColumnFactor.bWithQuotes = False
        ucrReceiverColumnFactor.SetParameterIncludeArgumentName(False)

        ucrReceiverWeights.SetParameter(New RParameter("weight.by", 3))
        ucrReceiverWeights.SetParameterIsRFunction()

        ucrChkRow.SetParameter(New RParameter("show.row.prc", 5), bNewChangeParameterValue:=True, bNewAddRemoveParameter:=True, strNewValueIfChecked:="TRUE", strNewValueIfUnchecked:="FALSE")
        ucrChkRow.SetText("Row (%)")
        ucrChkRow.SetRDefault("FALSE")

        ucrChkCount.SetParameter(New RParameter("show.obs", 6), bNewChangeParameterValue:=True, bNewAddRemoveParameter:=True, strNewValueIfChecked:="TRUE", strNewValueIfUnchecked:="FALSE")
        ucrChkCount.SetText("Count")
        ucrChkCount.SetRDefault("TRUE")

        ucrChkColumn.SetParameter(New RParameter("show.col.prc", 7), bNewChangeParameterValue:=True, bNewAddRemoveParameter:=True, strNewValueIfChecked:="TRUE", strNewValueIfUnchecked:="FALSE")
        ucrChkColumn.SetText("Column (%)")
        ucrChkColumn.SetRDefault("FALSE")

        ucrChkCell.SetParameter(New RParameter("show.cell.prc", 8), bNewChangeParameterValue:=True, bNewAddRemoveParameter:=True, strNewValueIfChecked:="TRUE", strNewValueIfUnchecked:="FALSE")
        ucrChkCell.SetText("Cell (%)")
        ucrChkCell.SetRDefault("FALSE")

        ucrChkWeights.SetText("Weights:")
        ucrChkWeights.SetParameter(ucrReceiverWeights.GetParameter(), bNewChangeParameterValue:=False, bNewAddRemoveParameter:=True)
        ucrChkWeights.AddToLinkedControls(ucrReceiverWeights, {True}, bNewLinkedAddRemoveParameter:=True, bNewLinkedHideIfParameterMissing:=True)

        ucrChkFlip.SetParameter(New RParameter("coord.flip", 4), bNewChangeParameterValue:=True, bNewAddRemoveParameter:=True, strNewValueIfChecked:="TRUE", strNewValueIfUnchecked:="FALSE")
        ucrChkFlip.SetText("Flip Coordinates")
        ucrChkFlip.SetRDefault("FALSE")

        ucrPnlFrequencyDisplay.AddRadioButton(rdoTable)
        ucrPnlFrequencyDisplay.AddRadioButton(rdoGraph)
        ucrPnlFrequencyDisplay.AddFunctionNamesCondition(rdoTable, "sjtab")
        ucrPnlFrequencyDisplay.AddFunctionNamesCondition(rdoGraph, "sjplot")
        ucrPnlFrequencyDisplay.AddToLinkedControls(ucrChkCount, {rdoTable}, bNewLinkedHideIfParameterMissing:=True)
        ucrPnlFrequencyDisplay.AddToLinkedControls(ucrChkRow, {rdoTable}, bNewLinkedHideIfParameterMissing:=True)
        ucrPnlFrequencyDisplay.AddToLinkedControls(ucrChkCell, {rdoTable}, bNewLinkedHideIfParameterMissing:=True)
        ucrPnlFrequencyDisplay.AddToLinkedControls(ucrChkColumn, {rdoTable}, bNewLinkedHideIfParameterMissing:=True)
        ucrPnlFrequencyDisplay.AddToLinkedControls(ucrPnlFreqType, {rdoGraph}, bNewLinkedHideIfParameterMissing:=True)

        ucrChkWeights.Enabled = False
        cmdOptions.Enabled = False

        ucrPnlFreqType.SetParameter(New RParameter("fun", 4))
        ucrPnlFreqType.AddRadioButton(rdoCount, Chr(34) & "grpfrq" & Chr(34))
        ucrPnlFreqType.AddRadioButton(rdoRow, Chr(34) & "xtab" & Chr(34))
        ucrPnlFreqType.AddRadioButton(rdoColumn, Chr(34) & "xtab" & Chr(34))
        ucrPnlFreqType.AddRadioButton(rdoCell, Chr(34) & "xtab" & Chr(34))

        ucrPnlFreqType.AddParameterPresentCondition(rdoCount, "margin", False)
        ucrPnlFreqType.AddParameterValuesCondition(rdoRow, "margin", Chr(34) & "row" & Chr(34))
        ucrPnlFreqType.AddParameterValuesCondition(rdoCell, "margin", Chr(34) & "cell" & Chr(34))
        ucrPnlFreqType.AddParameterValuesCondition(rdoColumn, "margin", Chr(34) & "col" & Chr(34))
    End Sub

    Private Sub SetDefaults()
        clsSelect = New RFunction
        clsSjPlot = New RFunction
        clsGroupBy = New RFunction
        clsSjTab = New RFunction
        clsTableBaseOperator = New ROperator
        clsGraphBaseOperator = New ROperator

        clsTableBaseOperator.SetOperation("%>%")
        clsTableBaseOperator.AddParameter("group_by", clsRFunctionParameter:=clsGroupBy, iPosition:=1)
        clsTableBaseOperator.AddParameter("select", clsRFunctionParameter:=clsSelect, iPosition:=2)
        clsTableBaseOperator.AddParameter("sjtab", clsRFunctionParameter:=clsSjTab, iPosition:=3)

        clsGraphBaseOperator.SetOperation("%>%")
        clsGraphBaseOperator.AddParameter("group_by", clsRFunctionParameter:=clsGroupBy, iPosition:=1)
        clsGraphBaseOperator.AddParameter("select", clsRFunctionParameter:=clsSelect, iPosition:=2)
        clsGraphBaseOperator.AddParameter("sjplot", clsRFunctionParameter:=clsSjPlot, iPosition:=3)

        clsGroupBy.SetPackageName("dplyr")
        clsGroupBy.SetRCommand("group_by")
        clsSelect.SetPackageName("dplyr")
        clsSelect.SetRCommand("select")

        ucrReceiverGroups.SetParameter(New RParameter("x"))
        ucrReceiverGroups.SetParameterIsString()
        ucrReceiverGroups.bWithQuotes = False
        ucrReceiverGroups.SetParameterIncludeArgumentName(False)

        ucrSelectorThreeVariableFrequencies.Reset()
        ucrReceiverGroups.SetMeAsReceiver()

        clsSjTab.SetPackageName("sjPlot")
        clsSjTab.SetRCommand("sjtab")
        clsSjTab.AddParameter("show.summary", "FALSE")
        clsSjTab.AddParameter("digits", 0)
        clsSjTab.AddParameter("fun", Chr(34) & "xtab" & Chr(34))

        clsSjPlot.SetPackageName("sjPlot")
        clsSjPlot.SetRCommand("sjplot")
        clsSjPlot.AddParameter("fun", Chr(34) & "grpfrq" & Chr(34))
        clsSjPlot.AddParameter("show.prc", "TRUE")
        clsSjPlot.AddParameter("show.n", "TRUE")
        ucrBase.clsRsyntax.SetBaseROperator(clsTableBaseOperator)
        bResetSubdialog = True
    End Sub

    'Still in progress
    Private Sub cmdOptions_Click(sender As Object, e As EventArgs) Handles cmdOptions.Click
        sdgTwoWayFrequencies.SetRFunction(clsSjTab, clsSjPlot, bResetSubdialog)
        bResetSubdialog = False
        sdgTwoWayFrequencies.ShowDialog()
        TestOkEnabled()
    End Sub

    Private Sub ucrChkFlip_ControlValueChanged(ucrChangedControl As ucrCore) Handles ucrChkFlip.ControlValueChanged
        Dim clsRowParam As RParameter
        Dim clsColumnParam As RParameter

        clsRowParam = ucrReceiverRowFactor.GetParameter()
        clsColumnParam = ucrReceiverColumnFactor.GetParameter()
        If rdoTable.Checked Then
            If ucrChkFlip.Checked Then
                clsColumnParam.Position = 1
                clsRowParam.Position = 2
            Else
                clsRowParam.Position = 1
                clsColumnParam.Position = 2
            End If
            ucrReceiverRowFactor.SetParameter(clsRowParam)
            ucrReceiverColumnFactor.SetParameter(clsColumnParam)
        End If
    End Sub

    Private Sub TestOkEnabled()
        If Not ucrReceiverGroups.IsEmpty() AndAlso Not ucrReceiverRowFactor.IsEmpty() AndAlso Not ucrReceiverColumnFactor.IsEmpty Then
            If Not ucrChkWeights.Checked Then
                ucrBase.OKEnabled(True)
            Else
                If ucrChkWeights.Checked AndAlso Not ucrReceiverWeights.IsEmpty Then
                    ucrBase.OKEnabled(True)
                Else
                    ucrBase.OKEnabled(False)
                End If
            End If
        Else
            ucrBase.OKEnabled(False)
        End If
    End Sub

    Private Sub Controls_ControlContentsChanged(ucrChangedControl As ucrCore) Handles ucrReceiverColumnFactor.ControlContentsChanged, ucrReceiverRowFactor.ControlContentsChanged, ucrReceiverWeights.ControlContentsChanged, ucrChkWeights.ControlContentsChanged
        TestOkEnabled()
    End Sub

    Private Sub ucrBase_BeforeClickOk(sender As Object, e As EventArgs) Handles ucrBase.BeforeClickOk
        If rdoTable.Checked Then
            ucrBase.clsRsyntax.SetBaseROperator(clsTableBaseOperator)
            ucrBase.clsRsyntax.iCallType = 0
        ElseIf rdoGraph.Checked Then
            ucrBase.clsRsyntax.SetBaseROperator(clsGraphBaseOperator)
            ucrBase.clsRsyntax.iCallType = 3
        End If
    End Sub

    Private Sub ucrBase_ClickReset(sender As Object, e As EventArgs) Handles ucrBase.ClickReset
        SetDefaults()
        SetRCodeForControls(True)
        TestOkEnabled()
    End Sub

End Class