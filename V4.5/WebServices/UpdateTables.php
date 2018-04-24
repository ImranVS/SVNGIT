<?php 
	
	require 'DBCreds.php';
    
	mysql_connect($server,$username,$password)
		or die ("Could not connect to the server");

	mysql_select_db($db) or die ("Couldn't connect to the database");
	mysql_set_charset("UTF8");

	$ans1 = mysql_query("select distinct OSType, TranslatedValue,
		OSName from OSTypeTranslation")
			or die(mysql_error());

	$ans2 = mysql_query("select distinct DeviceType,
		TranslatedValue, OSName from DeviceTypeTranslation")
		   	or die(mysql_error());

	$ans3 = mysql_query("select distinct Country,State from
		ValidLocations where Country is not null and State is not null
		and State <> '' and Country <> ''")
			or die(mysql_error());

	$arr1 = array();
	
	while($row=mysql_fetch_assoc($ans1)){
		$arr1[]=$row;
	}
	
	$arr2 = array();

        while($row=mysql_fetch_assoc($ans2)){
		$arr2[]=$row;
        }
	
	$arr3 = array();
	while($row=mysql_fetch_assoc($ans3)){
		$arr3[]=$row;
	}
	
	$arr = array();
	$arr['OS'] = $arr1;
	$arr['Device'] = $arr2;
	$arr['Location'] = $arr3;
	echo json_encode($arr);
?>
