﻿Imports System.Xml.Serialization

Namespace XMLModel
    <Serializable()> _
    Public Class SubtypeRelationship

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _ParentEntityTypeId As String
        <XmlAttribute()> _
        Public Property ParentEntityTypeId() As String
            Get
                Return Me._ParentEntityTypeId
            End Get
            Set(ByVal value As String)
                Me._ParentEntityTypeId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _SubtypingFactTypeId As String
        <XmlAttribute()>
        Public Property SubtypingFactTypeId() As String
            Get
                Return Me._SubtypingFactTypeId
            End Get
            Set(ByVal value As String)
                Me._SubtypingFactTypeId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsPrimarySubtypeRelationship As Boolean = False
        <XmlAttribute()>
        Public Property IsPrimarySubtypeRelationship() As Boolean
            Get
                Return Me._IsPrimarySubtypeRelationship
            End Get
            Set(ByVal value As Boolean)
                Me._IsPrimarySubtypeRelationship = value
            End Set
        End Property

    End Class

End Namespace
