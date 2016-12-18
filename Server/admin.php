<?php
header("Content-Type: text/html; charset=utf-8");
if($_SERVER['REQUEST_METHOD']=='POST'){

	$link = mysqli_connect('localhost', 'admin', '1234','ninja');
	 /* check connection */
	if (mysqli_connect_errno()) {
		printf("Connect failed: %s\n", mysqli_connect_error());
		exit();
	}
	$query = "SELECT * FROM `admin`  WHERE `login` = '".htmlspecialchars($_POST['login'])."'";
	$result = mysqli_query($link, $query);
	$line = mysqli_fetch_assoc($result);
	if (($line['pass']==md5($_POST['pass'])) and ($line['pass']!='')){
		session_start();
		$_SESSION['test']=time()+60*60;
		header("Location: /list.php");
		exit();
		//echo 'kdfs';
		//echo $line['pass'];
	}
	else{
//echo md5($_POST['pass']);
	}}

?>
<!DOCTYPE html>
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
	<title>ADMIN PANEL</title>
	<link rel="stylesheet" href="css/reset.css">
	<link rel="stylesheet" href="css/animate.css">
	<link rel="stylesheet" href="css/styles.css">
	<link rel="icon" href="img/favicon.ico" type="image/x-icon">
	<link rel="shortcut icon" href="img/favicon.ico" type="image/x-icon">
</head>
<body>
	<div id="container">
		<form action="admin.php" method="POST" name="insert">
			<label for="name">Login:</label>
			<input type="name"  name="login" value="<?php echo $_POST['login']?>">
			<label for="username">Password:</label>
			<input type="password" name="pass">
			<?php if ($line['pass']!=$_POST['pass'])
	{echo "<p>Неправильный пароль</p>";}?>  

			<div id="lower">
				<input type="submit" value="GO">
			</div>
		</form>
	</div>
</body>
</html>