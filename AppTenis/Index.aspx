<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AppTenis.Index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ingresa al sistema</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            background-color: #f2f2f2; /* Color de fondo */
        }

        .login-form {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: #fff;
            padding: 40px;
            border-radius: 10px;
            box-shadow: 0 10px 20px rgba(0, 0, 0, 0.3);
        }

        .login-text {
            color: #333;
            font-size: 2rem;
            font-weight: 500;
            text-align: center;
            margin-bottom: 20px;
        }

        .login-username,
        .login-password {
            width: 100%;
            padding: 10px;
            margin-bottom: 20px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

        .login-submit {
            width: 100%;
            background: #007bff; /* Color azul */
            border: none;
            padding: 15px;
            border-radius: 5px;
            color: #fff;
            font-size: 16px;
            cursor: pointer;
        }

        .login-submit:hover {
            background: #0056b3; /* Color azul más oscuro al pasar el mouse */
        }

        .user-image {
            display: block;
            margin: 0 auto 20px; /* Centra la imagen */
            width: 100px; /* Ajusta el ancho de la imagen según necesites */
            height: auto; /* Mantiene la proporción de la imagen */
            border-radius: 50%; /* Para que la imagen sea circular */
        }
    </style>
</head>
<body>
    <form runat="server" class="login-form">
        <p class="login-text">
            <span class="fa-stack fa-lg">
                <i class="fa fa-circle fa-stack-2"></i>
                <i class="fa fa-lock fa-stack-1x"></i>
            </span>
        </p>
        <img src="https://e7.pngegg.com/pngimages/338/430/png-clipart-computer-icons-user-login-swiggy-blue-text.png" alt="Imagen de usuario" class="user-image" /> <!-- Reemplaza URI_DE_LA_IMAGEN con la URI de tu imagen de usuario -->
        <asp:TextBox runat="server" ID="txbCorreo_Electronico" CssClass="login-username" Placeholder="Email" Required="true" />
        <asp:TextBox runat="server" ID="txbPassword" CssClass="login-password" TextMode="Password" Placeholder="Password" Required="true" />
        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="login-submit" OnClick="btnLogin_Click" />
    </form>
</body>
</html>


