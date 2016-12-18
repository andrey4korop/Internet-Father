<?php 
header("Content-Type: text/html; charset=utf-8"); 
session_start(); 
if ($_SESSION['test']<time())
{
	header("Location: /admin.php");	
	exit();
}
$link = mysqli_connect('localhost', 'admin', '1234','ninja');
mysqli_set_charset($link, "utf8");
$query = "SELECT * FROM region";
$result = mysqli_query($link, $query);
$flag = 0;
if($_SERVER['REQUEST_METHOD']=='POST')
{
	$flag=1;
	$query = 'SELECT * FROM school WHERE `region` ='. $_POST['region'] ;
	$result_school = mysqli_query($link, $query);
	
	if ($_POST['delkey'])
	{
		
		$query = 'DELETE FROM `license`  WHERE  `id` ='. $_POST['delkey'] ;
		$resul = mysqli_query($link, $query);

	}
	if ($_POST['delcookie'])
	{
		
		$query = 'UPDATE  `license` SET  `cookie` = ""  WHERE  `id` ='. $_POST['delcookie'] ;
		$resul = mysqli_query($link, $query);

	}
	
	if ($_POST['UPDATE']=="UPDATE")
	{
		
		$query = 'UPDATE  `license` SET  `dcreate` =  '.($_POST["mouth"]*60*60*24*30+time()).' WHERE  `school` ='. $_POST['school'] ;
		$resul = mysqli_query($link, $query);

	}
	if ($_POST['ADD']=="ADD")
	{
		
		for ($j=1; $j<=$_POST['key_count'];$j++)
		{
			$chars = 'QWERTYUIOPASDFGHJKLZXCVBNM';
  			$numChars = strlen($chars);
  			$string = '';
			for ($i = 0; $i < 20; $i++) 
			{
    			$string .= substr($chars, rand(1, $numChars) - 1, 1);
  			}
			$query = "INSERT INTO `license` (`id`, `key`, `dcreate`, `cookie`, `school`) VALUES (NULL, '".$string."', '', '', '".$_POST['school']."');";
			$result_insert = mysqli_query($link, $query);
			
		}
		
		
	}


		
	
	
	if ($_POST["school"])
	{
		$flag=2;
		$query = 'SELECT * FROM license WHERE `school` ='. $_POST['school'] ;
		$result_keys = mysqli_query($link, $query);
	}
	
	
}
?>
<!DOCTYPE html>
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
	<title>ADMIN PANEL Safe Internet 1.0</title>
	<link rel="stylesheet" href="css/reset.css">
	<link rel="stylesheet" href="css/animate.css">
	<link rel="stylesheet" href="css/styles.css">
	<link rel="icon" href="img/favicon.ico" type="image/x-icon">
	<link rel="shortcut icon" href="img/favicon.ico" type="image/x-icon">
</head>
<body>
	<ul class="css-menu-1">
	<li><a href="#">Add site</a></li>
	<li><a href="#">List of the blocked sites</a></li>
	<li><a href="/exit.php">Вихід</a></li>
</ul>
<form action="add_generate_key.php" method="POST" name="insert">
<div id="fix">
	
		
		<label for="name">Виберіть школу</label>
		<p>Виберіть область</p>
		<select name="region" size="1" <?php if ($flag>0) echo " disabled " ?> >
		 
		<?php 
		while ($row = mysqli_fetch_assoc($result)) 
		{
			$selected="";
			if ($_POST["region"]==$row["id"])
			{
				$selected="selected";
			}
			$res="<option $selected value='".$row["id"]."'>".$row["name_region"]."</option>";
			echo $res;	
			$selected="";
		}
			
		?>
	
		</select>
		<?php 
		
		if ($flag>0)
		{
			echo "<input name='region' type='hidden' value='".$_POST['region']."'>";
			echo '<p>Виберіть школу</p><select name="school" size="1">';
			while ($row = mysqli_fetch_assoc($result_school)) 
			{
				$selected="";
				if ($_POST["school"]==$row["id"])
				{
					$selected="selected";
				}
				$res="<option $selected value='".$row["id"]."'>".$row["school_name"]."</option>";
				echo $res;	
			$selected="";
			}
			
		}
			
		echo "</select>";
		?>
		<div id="lower">
			<input type="submit" value="GO">
		</div>
		
	

	
</div>
<?php if ($flag==2) { ?>
<div id="listofsites">
	<table>
	<th>key</th><th>cookie</th><th>dcreate</th>
	
		<?php
			while ($row = mysqli_fetch_assoc($result_keys))
			{
			echo '<tr>';
    		echo '<td>'.$row['key'].'</td>
				<td>'.$row['cookie'].'</td>
				<td>'.$row['dcreate'].'</td>
				<td>
					<button type="submit" name="delcookie" value="'.$row['id'].'">delcookie</button>
				</td>
				<td>
					<button type="submit" name="delkey" value="'.$row['id'].'">deykey</button>
				</td>';
  			echo '</tr>';
			}
		?>
	</table>
	<p>Продлить лицензию на </p>
	<input type="number" name="mouth" >
	<input type="submit" name="UPDATE" value="UPDATE">
	<p>Добавить ключей </p>
	<input type="number" name="key_count" >
	<input type="submit" name="ADD" value="ADD">
</div>

<?php } ?>
</form>	
</body>
</html>