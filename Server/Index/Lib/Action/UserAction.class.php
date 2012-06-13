<?php
class UserAction extends Action {
	
    public function login(){
        $username = $_GET["username"];
        $password = $_GET["password"];
        if ($username == "" || $password == "") {
        	echo "0";
        } else {
        	$User = M ( "User" );
        	$map ['username'] = $username;
        	$map ['password'] = $password;
        	$pass = $User->where ( $map )->find ();
        	if ($pass) {
        		echo $pass["uid"];
        	} else {
        		echo "0";
        	}
        }
    }
    
    public function addnew(){
    	$username = $_GET["username"];
    	$password = $_GET["password"];

    	if ($username == "" || $password == "") {
    		echo "false";
    	} else {
    		$User = M ( "User" );
    		$User->username = $username;
    		$User->password = $password;
    		$result = $User->add();
    		if ($result) {
    			echo "true";
    		} else {
    			echo "false";
    		}
    	}
    }
    
    //得到patner的uid,失败返回0
    public function findpartner(){
    	$username = $_GET["username"];
    	if ($username == "") {
    		echo "0";
    	} else {
    		$User = M ( "User" );
    		$map ['username'] = $username;
    		$result = $User->where($map)->find();
    		if ($result) {
    			echo $result['uid'];
    		} else {
    			echo "0";
    		}
    	}
    }
    
    public function getmydrawnum()
    {
    	$uid = $_GET['uid'];
    	$Queun = M ( "Queun" );
    	$map ['receiver_uid'] = $uid;
    	$map ['isdone'] = "0";
    	$result = $Queun->where($map)->select();
    	if ($result) {
    		echo count($result);
    	} else {
    		echo "0";
    	}
    }
    
    public function getmydrawthing()
    {
    	$uid = $_GET['uid'];
    	$Queun = M ( "Queun" );
    	$map ['receiver_uid'] = $uid;
    	$map ['isdone'] = "0";
    	$result = $Queun->where($map)->find();
    	if ($result) {
    		echo $result['drawthing'];
    	} else {
    		echo "0";
    	}
    }
    
    public function getsenderusername()
    {
    	$uid = $_GET['uid'];
    	$Queun = M ( "Queun" );
    	$map ['receiver_uid'] = $uid;
    	$map ['isdone'] = "0";
    	$result = $Queun->where($map)->find();
    	$sender_uid = $result['sender_uid'];
    	$User = M ('User');
    	$result = $User->find($sender_uid);
    	if ($result) {
    		echo $result['username'];
    	} else {
    		echo "0";
    	}
    }
    
    public function getmydrawxml()
    {
    	$uid = $_GET['uid'];
    	$Queun = M ( "Queun" );
    	$map ['receiver_uid'] = $uid;
    	$map ['isdone'] = "0";
    	$result = $Queun->where($map)->find();
    	if ($result) {
    		header("Content-Type:text/xml; charset=utf-8");
    		echo $result['xmlbody'];
    	} else {
    		echo "0";
    	}
    }
    
    public function updatemyscore()
    {
    	$uid = $_GET['uid'];
    	$score = $_GET['score'];
    	$User = M ("User");
    	$User->score = $score;
    	$result = $User->where("uid=$uid")->save();
    	if ($result) {
    		echo "true";
    	} else {
    		echo "false";
    	}
    }
}