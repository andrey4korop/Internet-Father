<?php
//if( $_SERVER['HTTP_USER_AGENT']=='Mozilla/3.0 (compatible; Indy Library)'){
//echo time()+60*60*24*10;
$link = mysqli_connect('localhost', 'admin', '1234','ninja');

//echo $_GET["id"];
//echo $_GET["val"];
function cookie($key,$cookie,$cookie_get) 
{
	if ($cookie=='')
	{
		$link = mysqli_connect('localhost', 'admin', '1234','ninja');
		$query = "UPDATE  `license` SET  `cookie` = '".$cookie_get."'  WHERE  `key` = '". $key."'";
		$resul_cookie = mysqli_query($link, $query);
		return "yes";
	}
	else
	{
		if ($cookie==$cookie_get)
		{
			return "yes";
		}
		else 
		{
			echo "cookie_no";
			return "no";
		}
	}	
  
}



if($_GET["id"]=="license")
{
	//$query = "SELECT dcreate FROM licenses WHERE `key` = '1234'";
	$query = "SELECT * FROM license  WHERE `key` = '".$_GET['val']."'";
	$result = mysqli_query($link, $query);
	$line = mysqli_fetch_assoc($result);	
	//echo $line['dcreate'];
	if (time()< $line['dcreate'])
	{	
		if (cookie($line['key'],$line['cookie'], $_GET['cookie'])=="yes")
		{
			echo "license_yes";
		}
	}	
	else
	{
		echo "license_no";
	}
}
if($_GET["id"]=="key")
{
	//$query = "SELECT dcreate FROM licenses WHERE `key` = '1234'";
	$query = "SELECT * FROM license  WHERE `key` = '".$_GET['val']."'";
	$result = mysqli_query($link, $query);
	$line = mysqli_fetch_assoc($result);	
	//echo $line['dcreate'];
	if (time()< $line['dcreate'])
	{
		if (cookie($line['key'],$line['cookie'], $_GET['cookie'])=='yes')
		{
			echo "key_yes";
		}
	}	
	else
	{	
		echo "key_no";
	}
}
if($_GET["id"]=="ver_black")
{
	//$query = "SELECT dcreate FROM licenses WHERE `key` = '1234'";
	$query = "SELECT * FROM setings  WHERE `id` = '1'";
	$result = mysqli_query($link, $query);
	$line = mysqli_fetch_assoc($result);	
	//echo $line['dcreate'];
	echo $line['value'];	
	
}
if($_GET["id"]=="black")
{
	//$query = "SELECT dcreate FROM licenses WHERE `key` = '1234'";
	$query = "SELECT * FROM black";
	$result = mysqli_query($link, $query);
	
	while($line = mysqli_fetch_assoc($result))
	{	
	//echo $line['dcreate'];
	echo $line['site'];
	echo "
";
	
	//}
}

// Освобождаем память от результата
//mysql_free_result($result);

// Закрываем соединение
mysqli_close($link);
}
?>