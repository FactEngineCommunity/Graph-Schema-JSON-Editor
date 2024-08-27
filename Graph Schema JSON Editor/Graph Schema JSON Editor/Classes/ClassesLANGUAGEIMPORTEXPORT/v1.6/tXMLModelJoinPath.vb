﻿Imports System.Xml.Serialization

Namespace XMLModel16

    <Serializable()>
    Public Class JoinPath

        ''' <summary>
        ''' The set of Roles traversed in order to form the JoinPath.
        ''' </summary>
        ''' <remarks></remarks>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _RolePath As New List(Of XMLModel16.RoleReference)
        Public Property RolePath As List(Of XMLModel16.RoleReference)
            Get
                Return Me._RolePath
            End Get
            Set(value As List(Of XMLModel16.RoleReference))
                Me._RolePath = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _JoinPathError As pcenumJoinPathError = pcenumJoinPathError.None
        <XmlAttribute()>
        Public Property JoinPathError As pcenumJoinPathError
            Get
                Return Me._JoinPathError
            End Get
            Set(value As pcenumJoinPathError)
                Me._JoinPathError = value
            End Set
        End Property

    End Class

End Namespace
