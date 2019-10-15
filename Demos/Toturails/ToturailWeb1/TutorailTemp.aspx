<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TutorailTemp.aspx.cs" Inherits="ToturailWeb1.TutorailTemp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/style-min.css" rel="stylesheet" />
    <%--<script type="text/javascript" src="Scripts/script-min-v4.js?v=2"></script>--%>
</head>
<body>
    <div class="wrapLoader">
        <div class="imgLoader">
            <img src="/images/loading-cg.gif" alt="" width="70" height="70">
        </div>
    </div>
    <div id="right_obs" class="display-none" onclick="close_obs_sidenav()"></div>
    <header>
        <div class="container">
            <h1 class="logo">
                <a href="index.htm" title="哈喽教程">
                    哈喽教程
                </a>
            </h1>
            <ul class="tp-inline-block pull-right" id="tp-head-icons">
                <li>
                    <div class="tp-second-nav tp-display-none tp-pointer" onclick="openNav()">
                        <i class="fa fa-th-large fa-lg"></i>
                    </div>
                </li>
            </ul>
            <button class="btn btn-responsive-nav btn-inverse" data-toggle="collapse" data-target=".nav-main-collapse" id="pull" style="top: 290px!important"> <i class="icon icon-bars"></i> </button>
        </div>
        <div class="sidenav" id="mySidenav">
            <div class="navbar nav-main">
                <div class="container">
                    <nav class="nav-main mega-menu">
                        <ul class="nav nav-pills nav-main" id="mainMenu">
                            <li class="dropdown no-sub-menu">
                                <!--<div class="searchform-popup">
                                    <input class="header-search-box" type="text" id="search-string" name="q" placeholder="Search your favorite tutorials..." onfocus="if (this.value == 'Search your favorite tutorials...') {this.value = '';}" onblur="if (this.value == '') {this.value = 'Search your favorite tutorials...';}" autocomplete="off" style="">
                                    <div class="magnifying-glass"><i class="icon-search"></i> Search </div>
                                </div>-->
                            </li>
                        </ul>
                    </nav>
                </div>
            </div>
        </div>

    </header>
    <div style="clear:both;"></div>
    <div role="main" class="main">
        <div class="container">
            <div class="row">
                <div class="col-md-2">
                    <aside class="sidebar">
                        <% foreach(var group in dicMenu){ %>
                        <ul class="nav nav-list primary left-menu">
                            <li class="heading"><%= group.Key %></li>
                            <% foreach(var chapter in group) {%>
                            <li><a target="_top" href="<%= chapter.original_url %>" style="background-color: rgb(214, 214, 214);"><%= chapter.chapter_name %></a></li>
                            <%} %>
                        </ul>
                        <%} %>
                    </aside>
                </div>
                <!-- PRINTING STARTS HERE -->
                <div class="row">
                    <div class="content">
                        <!-- MAIN CONTENT -->
                        <%=chapterContent.charpter_content %>
                    </div>
                    <div class="row">
                        <div class="col-md-2" id="rightbar">
                            <div class="simple-ad">
                                主目录
                            </div>
                            <div class="rightgooglead">
                            广告
                            </div>
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="footer-copyright">
            <div class="container">
                <div class="row">
                   
                </div>
            </div>
        </div>
    </div>
    <div id="privacy-banner" style="display: none;">
        <div>
            <p>
                We use cookies to provide and improve our services. By using our site, you consent to our Cookies Policy.
                <a id="banner-accept" href="javascript:void(0)">Accept</a>
                <a id="banner-learn" href="/about/about_cookies.htm" target="_blank">Learn more</a>
            </p>
        </div>
    </div>
</body>
</html>
