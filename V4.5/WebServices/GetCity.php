<?php session_start();

	require 'DBCreds.php';
						
	mysql_connect($server,$username,$password)
		or die ("Could not connect to the server");

	mysql_select_db($db) or die ("Couldn't connect to the database");
	mysql_set_charset("UTF8");

	$State = $_GET['State'];
	$Country = $_GET['Country'];
	
	$ans = mysql_query("SELECT distinct City FROM ValidLocations where City <> '' AND State ='" . $State . "' AND Country='" . $Country . "' ORDER BY City")
		or die(mysql_error());
		
	$arr = array();
	
	while($row=mysql_fetch_assoc($ans)){
		$arr[]=$row;
	}

	echo
	json_encode($arr);
?>
