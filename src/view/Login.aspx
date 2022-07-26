<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="asf.cms.view.Login" EnableViewStateMac="false" EnableEventValidation="false" EnableViewState="false" ValidateRequest="true"  %>

<!DOCTYPE html>

<html>
<head>
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">

	<title>ASF - Auditoria Superior de la Federación / CMS</title>
	
	<link rel="stylesheet" type="text/css" href="../view/lib/fomantic-ui-2.8.4/semantic.min.css" />
    <link rel="stylesheet" type="text/css" href="../view/css/admin/admin_extra_v3.css" />
	
	<script type="text/javascript" src="../view/lib/jquery3.4.1/jquery-3.4.1.min.js"></script>
	<script type="text/javascript" src="../view/lib/fomantic-ui-2.8.4/semantic.min.js"></script>
	<script type="text/javascript" src="../view/js/admin/v3/commons3.js"></script>
	<script type="text/javascript" src="../view/js/admin/v3/SimpleDialogBox.js"></script>
	<script type="text/javascript" src="../view/js/admin/v3/ResultMessages.js"></script>
	<script type="text/javascript" src="../view/js/admin/v3/Labels.js"></script>
</head>
<body>
	<div class="ui middle aligned center aligned grid" style="height: 100%">

		<div class="six wide left aligned column">
			<h1>Sistema de Manejo de Contenidos ASF</h1>
			<p>
				Bienvenido al sistema de manejo de contenidos del sitio de la <strong>Auditoria Superior de la Federaci&oacute;n</strong>.
				Por favor ingrese sus datos para accesar al sistema.
			</p>
			<form class="ui form" id="form1" action="../Login/Authenticate" runat="server">
			
				<span style="color:Red"><%=GetMessage("msg") %></span>

				<div class="ui field">
					<label>Nombre de usuario</label>
					<input type="text" id="login" name="login" />
				</div>
				<div class="ui field">
					<label>Contraseña</label>
					<input type="password" id="password" name="password" />
				</div>

				<button class="ui primary button" value="submit">Acceder</button>
                
            </form>
		</div>
	</div>
</body>
</html>
   