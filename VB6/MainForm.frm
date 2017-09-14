VERSION 5.00
Begin VB.Form MainForm 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Subsriber"
   ClientHeight    =   5250
   ClientLeft      =   45
   ClientTop       =   390
   ClientWidth     =   5535
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5250
   ScaleWidth      =   5535
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton Command1 
      Caption         =   "Initialize"
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   1455
   End
   Begin VB.ListBox MessageList 
      Height          =   4155
      ItemData        =   "MainForm.frx":0000
      Left            =   120
      List            =   "MainForm.frx":0002
      TabIndex        =   0
      Top             =   960
      Width           =   5295
   End
End
Attribute VB_Name = "MainForm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public WithEvents client As RabbitClient
Attribute client.VB_VarHelpID = -1

Private Sub client_MessageReceived(ByVal e As VB6ComLib.MessageEventArgs)
MessageList.AddItem e.Message, 0
End Sub

Private Sub Command1_Click()
Dim result
result = client.Initialize("13.65.87.69", "hello")
End Sub

Private Sub Form_Load()

Set client = New RabbitClient



End Sub

Private Sub Form_Terminate()
Dim result
result = client.Dispose()
End Sub

