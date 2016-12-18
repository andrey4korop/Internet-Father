<?php 
header("Content-Type: text/html; charset=utf-8"); 
session_start(); 
if ($_SESSION['test']<time()){
header("Location: /admin.php");	
exit();
}
$link = mysqli_connect('localhost', 'admin', '1234','ninja');
if ($_GET['del'])
{
	$query = "DELETE FROM `black` WHERE id='".$_GET['del']."'";
	$result = mysqli_query($link, $query);
}
if($_SERVER['REQUEST_METHOD']=='POST')
{
	$link = mysqli_connect('localhost', 'admin', '1234','ninja');
	$q=0;
	$v=0;
	$arrTextarea = explode("\n", str_replace(array("\r\n", "\n\r"), "\n", $_POST['comment']));
	for($z = 0, $cnt = count($arrTextarea); $z < $cnt; $z++)
	{
		$query = "SELECT id FROM black WHERE `site` = '".$arrTextarea[$z]."'";
		$result = mysqli_query($link, $query);
		if($line = mysqli_fetch_assoc($result))
		{
		$q++;
		}
		else
		{
		$query  ="INSERT INTO `black`(`site`) VALUES ('".$arrTextarea[$z]."')";
		$result = mysqli_query($link, $query);
		$v++;
		} 
	}
		$query = "SELECT `value` FROM `setings`  WHERE `id` = '1'";
	$result = mysqli_query($link, $query);
	$line = mysqli_fetch_assoc($result);
	$w=$line['value']+$v;
	$query="UPDATE `setings` SET `value`='".$w."' WHERE `id` = '1'";
	$result = mysqli_query($link, $query);
}
	
$num = 20;
$page = $_GET['page'];
$result00 = mysqli_query($link,"SELECT COUNT(*) FROM black");
$temp = mysqli_fetch_array($result00);
$posts = $temp[0];
$total = (($posts - 1) / $num) + 1;
$total =  intval($total);
$page = intval($page);
if(empty($page) or $page < 0) $page = 1;
if($page > $total) $page = $total;
$start = $page * $num - $num;		
		
$result = mysqli_query($link,"SELECT * FROM black ORDER BY id LIMIT $start, $num");
 $table="";

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
<div id="fix">
		<form action="list.php" method="POST" name="insert">
		
		<label for="name">Добавити сайти:</label>
		<p>Приклад: example.com</p>
		<p>неправильно: http://example.com/index.php</p>
			<textarea name="comment" cols="40" rows="10"></textarea>
<?php
if($_SERVER['REQUEST_METHOD']=='POST')
{	
echo "<p>Із $z сайтій було добавлено $v, а $q вже були в базі</p>";
}

?>			
			<div id="lower">
				<input type="submit" value="GO">
			</div>
		
	

</form>
	</div>
<div id="listofsites">
		<form>
			<table>
<?php 
 while($row = mysqli_fetch_array($result)) {                   //цикл
 $table .= "<tr >";
 $table .= "<td >".$row['id']."</td>";
 $table .= "<td >".$row['site']."</td>";
 $table .= "<td ><a href=list.php?page=".$page."&del=".$row['id'].">del</a></td>";
 $table .= "</tr>";
 }
 echo $table;
 ?>
</table>
<?php
// Проверяем нужны ли стрелки назад
if ($page != 1) $pervpage = '<a href=list.php?page=1>Первая</a> | <a href=list.php?page='. ($page - 1) .'>Предыдущая</a> | ';
// Проверяем нужны ли стрелки вперед
if ($page != $total) $nextpage = ' | <a href=list.php?page='. ($page + 1) .'>Следующая</a> | <a href=list.php?page=' .$total. '>Последняя</a>';

// Находим две ближайшие станицы с обоих краев, если они есть

if($page - 3 > 0) $page3left = ' <a href=list.php?page='. ($page - 3) .'>'. ($page - 3) .'</a> | ';
if($page - 2 > 0) $page2left = ' <a href=list.php?page='. ($page - 2) .'>'. ($page - 2) .'</a> | ';
if($page - 1 > 0) $page1left = '<a href=list.php?page='. ($page - 1) .'>'. ($page - 1) .'</a> | ';

if($page + 3 <= $total) $page3right = ' | <a href=list.php?page='. ($page + 3) .'>'. ($page + 3) .'</a>';
if($page + 2 <= $total) $page2right = ' | <a href=list.php?page='. ($page + 2) .'>'. ($page + 2) .'</a>';
if($page + 1 <= $total) $page1right = ' | <a href=list.php?page='. ($page + 1) .'>'. ($page + 1) .'</a>';

// Вывод меню если страниц больше одной

if ($total > 1)
{
Error_Reporting(E_ALL & ~E_NOTICE);
echo '<div class="pstrnav">';
echo $pervpage.$page5left.$page4left.$page3left.$page2left.$page1left.'<b>'.$page.'</b>'.$page1right.$page2right.$page3right.$page4right.$page5right.$nextpage;
echo "</div>";
}
?>
		</form>
	</div>
	
</body>
</html>