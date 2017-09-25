VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form MainForm 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Subsriber"
   ClientHeight    =   6210
   ClientLeft      =   45
   ClientTop       =   390
   ClientWidth     =   5535
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   6210
   ScaleWidth      =   5535
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton btnReply 
      Caption         =   "Reply"
      Enabled         =   0   'False
      Height          =   375
      Left            =   240
      TabIndex        =   6
      Top             =   5640
      Width           =   1815
   End
   Begin VB.TextBox txtResponse 
      Enabled         =   0   'False
      Height          =   1335
      Left            =   240
      MultiLine       =   -1  'True
      TabIndex        =   5
      Top             =   4200
      Width           =   5055
   End
   Begin MSComctlLib.ListView MessagesView 
      Height          =   2655
      Left            =   240
      TabIndex        =   1
      Top             =   720
      Width           =   5055
      _ExtentX        =   8916
      _ExtentY        =   4683
      View            =   3
      LabelEdit       =   1
      LabelWrap       =   -1  'True
      HideSelection   =   -1  'True
      FullRowSelect   =   -1  'True
      GridLines       =   -1  'True
      _Version        =   393217
      ForeColor       =   -2147483640
      BackColor       =   -2147483643
      BorderStyle     =   1
      Appearance      =   1
      Enabled         =   0   'False
      NumItems        =   1
      BeginProperty ColumnHeader(1) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         Text            =   "Message"
         Object.Width           =   2540
      EndProperty
   End
   Begin VB.CommandButton btnInitialize 
      Caption         =   "Initialize"
      Height          =   375
      Left            =   360
      TabIndex        =   0
      Top             =   120
      Width           =   1455
   End
   Begin VB.Label Label2 
      Caption         =   "Response:"
      Height          =   255
      Left            =   240
      TabIndex        =   4
      Top             =   3840
      Width           =   975
   End
   Begin VB.Label lblCorrelationId 
      Height          =   255
      Left            =   1560
      TabIndex        =   3
      Top             =   3480
      Width           =   3615
   End
   Begin VB.Label Label1 
      Caption         =   "Correlation id:"
      Height          =   255
      Left            =   240
      TabIndex        =   2
      Top             =   3480
      Width           =   1215
   End
End
Attribute VB_Name = "MainForm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public WithEvents client As RabbitClient
Attribute client.VB_VarHelpID = -1
Private Declare Sub Sleep Lib "kernel32.dll" (ByVal dwMilliseconds As Long)
Private currentCorrId As String


Private Sub btnInitialize_Click()
Dim result
result = client.Initialize("13.65.87.69", "hello")
MessagesView.Enabled = True
End Sub

Private Sub btnReply_Click()
Dim result
result = client.Reply(currentCorrId, txtResponse.Text)

If result <> "OK" Then MsgBox result, vbCritical, "Error"

'Clear
MessagesView.ListItems.Remove (currentCorrId)
txtResponse.Text = ""
lblCorrelationId.Caption = ""
txtResponse.Enabled = False
btnReply.Enabled = False

End Sub

Private Sub client_MessageReceived(ByVal e As VB6ComLib.MessageEventArgs)
MessagesView.ListItems.Add 1, e.correlationId, e.Message
End Sub

Private Sub Form_Load()

Set client = New RabbitClient
End Sub

Private Sub Form_Terminate()
Dim result
result = client.Dispose()
End Sub

Private Sub MessagesView_ItemClick(ByVal Item As MSComctlLib.ListItem)
currentCorrId = Item.Key
lblCorrelationId.Caption = currentCorrId

txtResponse.Enabled = True
btnReply.Enabled = True

End Sub
