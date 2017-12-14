<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <%--Add for Bingmap--%>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script charset="UTF-8" type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0"></script>
    <script type="text/javascript">
//        var map = null;
//        var isExecuteFuction = "false";
//        // Define colors
//        var pushPinNum = 0;
//        var pushPins_Array = new Array();
//        var pinInfoBoxs_Array = new Array();
//        var BoundingboxPolygon_Array = new Array();
//        var currentHighlightPushPinIndex = -1;

//        var blue = new Microsoft.Maps.Color(100, 0, 0, 200);
//        var green = new Microsoft.Maps.Color(100, 0, 100, 100);
//        var purple = new Microsoft.Maps.Color(100, 100, 0, 100);

//        var polygonLineColor = new Microsoft.Maps.Color(100, 100, 0, 100);
//        var polygonFillColor = new Microsoft.Maps.Color(20, 20, 0, 100);
//        var polygonHighlightFillColor = new Microsoft.Maps.Color(40, 40, 0, 100);
//        var bboxpolygonLineThickness = 2;

//        function GetMap() 
//        {
////            map = new Microsoft.Maps.Map(document.getElementById("mapDiv"), { credentials: "AuFyU6cj9lefQ5ZB5DTpktZmw_uwxqug3yK61ryx8zAXKjY27MIWuUfeFsXY0lc4" });
////            map = new Microsoft.Maps.Map(document.getElementById("mapDiv"),
////                           { credentials: "AuFyU6cj9lefQ5ZB5DTpktZmw_uwxqug3yK61ryx8zAXKjY27MIWuUfeFsXY0lc4",
////                               center: new Microsoft.Maps.Location(45.5, -122.5),
////                               mapTypeId: Microsoft.Maps.MapTypeId.road,
////                               zoom: 7
////                           });
////            map = new Microsoft.Maps.Map(document.getElementById("mapDiv"),
////                           { credentials: "AuFyU6cj9lefQ5ZB5DTpktZmw_uwxqug3yK61ryx8zAXKjY27MIWuUfeFsXY0lc4",
////                               center: new Microsoft.Maps.Location(38.49, -100.18),
////                               mapTypeId: Microsoft.Maps.MapTypeId.road,
////                               zoom: 5
////                           });

//                           var mapOptions = {
//                               credentials: "AuFyU6cj9lefQ5ZB5DTpktZmw_uwxqug3yK61ryx8zAXKjY27MIWuUfeFsXY0lc4",
//                               center: new Microsoft.Maps.Location(39.00, -100.00),
//                               mapTypeId: Microsoft.Maps.MapTypeId.road,
//                               zoom: 5,
//                               showScalebar: false
//                           }
//                           map = new Microsoft.Maps.Map(document.getElementById("mapDiv"), mapOptions);
//                           var viewBoundaries = Microsoft.Maps.LocationRect.fromLocations(new Microsoft.Maps.Location(90, -180), new Microsoft.Maps.Location(90, 180), new Microsoft.Maps.Location(-90, 180), new Microsoft.Maps.Location(-90, -180));
//                           map.setView({ bounds: viewBoundaries });
//          }

//          function SetZoom() 
//          {
//              var zoomLevel = parseInt(document.getElementById('txtZoom').value);
//              map.setView({ zoom: zoomLevel });
//          }

//          function ShowOrHideSilverlightControlHost() 
//          {
//              if (document.getElementById('silverlightControlHost').style.display == "none")
//                  document.getElementById('silverlightControlHost').style.display = ""
//              else
//                  document.getElementById('silverlightControlHost').style.display = "none"
//          }

//          function AddPushPinAndBBox(lower_lon, lower_lat, upper_lon, upper_lat, title1, privider) {

//              var lon = null;
//              if (lower_lon <= upper_lon) {
//                  lon = (lower_lon + upper_lon) / 2;
//              }
//              //当lower_lon大于upper_lon时，跨越反面子午线 Cross Antimeridian(而不是本初子午线prime meridian),中心点位置需要特别的计算
//              else {
//                  var crossed_lon_range = (180 - lower_lon)+ (180 + upper_lon);
//                  lon = lower_lon + crossed_lon_range / 2;
//                  if (lon > 180) 
//                  {
//                      lon = upper_lon - crossed_lon_range / 2;
//                  }

//              }

//              var lat = (lower_lat + upper_lat) / 2;
//              // Define the pushpin location
//              var loc = new Microsoft.Maps.Location(lat, lon);
//              // Add a pin to the map
//              var pin = new Microsoft.Maps.Pushpin(loc, { text: ''+(++pushPinNum)});
//              // Add a pin to the center of the map, using a custom icon
//              //var pin = new Microsoft.Maps.Pushpin(center, { icon: 'BluePushpin.png', width: 50, height: 50, draggable: true });
//              map.entities.push(pin);
//              // Center the map on the location
//              //map.setView({ center: loc, zoom: 10 });

//              // Create the infobox for the pushpin
//              var pinInfobox = new Microsoft.Maps.Infobox(pin.getLocation(),
//                { title: title1,
//                    description: 'Provider: '+privider + '<br/>BoundingBox: (' + lower_lon + ', ' + lower_lat + ', ' + upper_lon + ', ' + upper_lat + ')',
//                    visible: false,
//                    offset: new Microsoft.Maps.Point(0, 15)
//                });

//              // Add handler for the pushpin click event.
//                Microsoft.Maps.Events.addHandler(pin, 'click', highlightPushPinRelatedRecord);
//              // Hide the infobox when the map is moved.
//                Microsoft.Maps.Events.addHandler(map, 'click', hideInfobox);

//              // Add the pushpin and infobox to the map
//              map.entities.push(pinInfobox);
//              AddBBoxPolygon(lower_lon, lower_lat, upper_lon, upper_lat, polygonLineColor, polygonFillColor, bboxpolygonLineThickness);

//              pushPins_Array.push(pin);
//              pinInfoBoxs_Array.push(pinInfobox);
//          }

//          //高亮显示PushPin对应元数据的BBOX及显示基本信息框
//          function highlightPushPinRelatedRecord(e) 
//          {
//              var currentPin = e.target;
//              for (var i = 0; i < pushPins_Array.length; i++) 
//              {
//                  if (pushPins_Array[i] == currentPin) 
//                  {
//                      currentHighlightPushPinIndex = i;
//                      var currentPinInfoBoxes = pinInfoBoxs_Array[i];
//                      currentPinInfoBoxes.setOptions({ visible: true });
//                      BoundingboxPolygon_Array[i].setOptions({ fillColor: purple, strokeColor: purple });
//                      break;
//                  }
//              }
//          }

//          //高亮显示指定index的PushPin对应元数据的BBOX及显示基本信息框
//          function highlightSpecifiedRecord(index) {
//              if (pushPins_Array.length > 0) 
//              {
//                  currentHighlightPushPinIndex = index;
//                  var currentPinInfoBoxes = pinInfoBoxs_Array[index];
//                  currentPinInfoBoxes.setOptions({ visible: true });
//                  BoundingboxPolygon_Array[index].setOptions({ fillColor: purple, strokeColor: purple });

//                  for (var i = 0; i < pushPins_Array.length; i++) {
//                      if (i != index) {
//                          pin = pinInfoBoxs_Array[i].setOptions({ visible: false });
//                          BoundingboxPolygon_Array[i].setOptions({ fillColor: polygonFillColor, strokeColor: polygonLineColor });
//                      }
//                  }
//              }
//          }

//          //隐藏所有PushPin对应元数据的基本信息框
//          function hideInfobox(e) 
//          {
////              if (e.targetType == "map") 
////              {
////                  var point = new Microsoft.Maps.Point(e.getX(), e.getY());
////                  var loc = e.target.tryPixelToLocation(point);
////                  var pin = new Microsoft.Maps.Pushpin(loc);
////                  map.entities.push(pin);
////              }

////              var i = 0;
////              for (i = 0; i < pinInfoBoxs_Array.length; i++) 
////              {
////                  pin = pinInfoBoxs_Array[i].setOptions({ visible: false });
//              //              }
//              if (currentHighlightPushPinIndex != -1) {
//                  pin = pinInfoBoxs_Array[currentHighlightPushPinIndex].setOptions({ visible: false });
//                  BoundingboxPolygon_Array[currentHighlightPushPinIndex].setOptions({ fillColor: polygonFillColor, strokeColor: polygonLineColor });
//                  currentHighlightPushPinIndex = -1;
//              }
//          }

//          function AddBBoxPolygon(lower_lon, lower_lat, upper_lon, upper_lat, polygonLineColor, polygonFillColor, lineThickness) 
//          {
//              var middle_lon1 = null;
//              var middle_lon2 = null;
//              if (lower_lon <= upper_lon) {
//                   middle_lon1 = lower_lon + (upper_lon - lower_lon) / 3;
//                   middle_lon2 = lower_lon + 2*(upper_lon - lower_lon) / 3;
//              }
//              //当lower_lon大于upper_lon时，跨越反面子午线 Cross Antimeridian(而不是本初子午线prime meridian)
//              else {
//                   middle_lon1 = 180;
//                   middle_lon2 = (180 + upper_lon) / 2 - 180;
//              }

//              // Create the locations
//              var location1 = new Microsoft.Maps.Location(lower_lat, lower_lon);
//              var location2 = new Microsoft.Maps.Location(lower_lat, middle_lon1);
//              var location3 = new Microsoft.Maps.Location(lower_lat, middle_lon2);
//              var location4 = new Microsoft.Maps.Location(lower_lat, upper_lon);
//              var location5 = new Microsoft.Maps.Location(upper_lat, upper_lon);
//              var location6 = new Microsoft.Maps.Location(upper_lat, middle_lon2);
//              var location7 = new Microsoft.Maps.Location(upper_lat, middle_lon1);
//              var location8 = new Microsoft.Maps.Location(upper_lat, lower_lon);
//              var location9 = new Microsoft.Maps.Location(lower_lat, lower_lon);

//              // Create a polygon 
//              var vertices = new Array(location1, location2, location3, location4, location5, location6, location7, location8, location9);
//              var polygon = new Microsoft.Maps.Polygon(vertices, { fillColor: polygonFillColor, strokeColor: polygonLineColor, strokeThickness: lineThickness });
//              // Add the polygon to the map
//              map.entities.push(polygon);
//              BoundingboxPolygon_Array.push(polygon);
//          }

//          function AddEntities() 
//          {
//              // Create the locations
//              var location1 = new Microsoft.Maps.Location(20, -20);
//              var location2 = new Microsoft.Maps.Location(20, 20);
//              var location3 = new Microsoft.Maps.Location(-20, 20);
//              var location4 = new Microsoft.Maps.Location(-20, -20);
//              var location5 = new Microsoft.Maps.Location(40, 0);

//              // Create some shapes
//              var triangleVertices = new Array(location1, location2, location5, location1);
//              var triangle = new Microsoft.Maps.Polygon(triangleVertices, { fillColor: blue, strokeColor: blue });

//              var squareVertices = new Array(location1, location2, location3, location4, location1);
//              var square = new Microsoft.Maps.Polygon(squareVertices, { fillColor: purple, strokeColor: purple });

//              // Add the shapes to the map
//              map.entities.push(triangle);
//              map.entities.push(square);
//          }

//          function IsBingMapDIVVisible() 
//          {
//              if (document.getElementById('mapDiv').style.display == "none")
//                  return false;
//              else
//                  return true;          
//          }

//          function IsSilverlightControlHostDIVVisible() 
//          {
//              if (document.getElementById('silverlightControlHost').style.display == "none")
//                  return false;
//              else
//                  return true;
//          }

//          function IsDocumentElementVisible(elementName) 
//          {
//              var element = document.getElementById(elementName);
//              if (element != null) 
//              {
//                  if (element.style.display == "none")
//                      return false;
//                  else
//                      return true;
//              }
//          }

//          function initialMapEntitiesAndArrays()
//          { 
//              map.entities.clear();
//              pushPins_Array = new Array();
//              pinInfoBoxs_Array = new Array();
//              BoundingboxPolygon_Array = new Array();
//              pushPinNum = 0;
//              currentHighlightPushPinIndex = -1;
//          }

//          function ResizeBingMapFullScreenOrNormalSize() {
//              if (document.getElementById('silverlightControlHost').style.display == "none") {
//                  document.getElementById('silverlightControlHost').style.display = "";
//                  document.getElementById('silverlightControlHost').style.height = "60%";
//                  document.getElementById('mapDiv').style.height = "37%";
//                  document.getElementById('ResizeBingMap_FullScreen_NormalSize').value = "Full Screen Bing Map";
//              }
//              else {
//                  document.getElementById('silverlightControlHost').style.display = "none";
//                  document.getElementById('silverlightControlHost').style.height = "0%";
//                  document.getElementById('mapDiv').style.height = "97%";
//                  document.getElementById('ResizeBingMap_FullScreen_NormalSize').value = "Exit Full Screen";
//              }
//          }

//          function ShowNormalSizeBingMap() {
//              if (document.getElementById('mapDiv').style.display == "none") {
//                  document.getElementById('mapDiv').style.display = "";
//                  document.getElementById('silverlightControlHost').style.height = "60%";
//                  document.getElementById('mapDiv').style.height = "37%";
//                  document.getElementById('controlButtons').style.height = "3%";
//                  document.getElementById('controlButtons').style.display = "";
//                  document.getElementById('ResizeBingMap_FullScreen_NormalSize').value = "Full Screen Bing Map";
//              }
//          }

//          function HideBingMap() {
//              if (document.getElementById('mapDiv').style.display == "") {
//                  document.getElementById('mapDiv').style.display = "none";
//                  document.getElementById('silverlightControlHost').style.height = "100%";
//                  document.getElementById('mapDiv').style.height = "0%";
//                  document.getElementById('controlButtons').style.height = "0%";
//                  document.getElementById('controlButtons').style.display = "none";
//                  document.getElementById('ResizeBingMap_FullScreen_NormalSize').value = "Full Screen Bing Map";
//              }
//          }
      </script>
    <title>GeoSearch</title>
    <style type="text/css">
    html, body {
	    height: 100%;
	    overflow: auto;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 100%;
	    text-align:center;
    }
    </style>
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
              appSource = sender.getHost().Source;
            }
            
            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
              return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " +  appSource + "\n" ;

            errMsg += "Code: "+ iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {           
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " +  args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>
    <script type="text/javascript">
        var xmlHttp;
        var result;
        //创建一个httpAdapter对象
        function GethttpAdapterRequest() {
            //return window.ActiveXObject ? new ActiveXObject("Microsoft.XMLHTTP") : new XMLHttpRequest();
            if (window.ActiveXObject)
                xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
            else if (window.XMLHttpRequest)
                xmlHttp = new XMLHttpRequest();
        }

        //        function createQueryString() {
        //            var firstName = document.getElementById("firstName").value;
        //            var birthday = document.getElementById("birthday").value;
        //            var queryString = "firstName=" + firstName + "&birthday=" + birthday;
        //            return encodeURI(encodeURI(queryString)); //防止乱码
        //        }

        //            function doRequestUsingGet() {
        //                GethttpAdapterRequest();
        //                var queryString = "AjaxHandler.ashx?";
        //                queryString += createQueryString() + "&timestamp=" + new Date().getTime();
        //                xmlHttp.open("GET", queryString);
        //                xmlHttp.onreadystatechange = handleStateChange;
        //                xmlHttp.send(null);
        //            }

        function doRequestUsingPost(url, content) {
            GethttpAdapterRequest();
            xmlHttp.open("POST", url, false);

            //xmlHttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            xmlHttp.SetRequestHeader("Content-Type", "text/xml; charset=utf-8");
            xmlHttp.onreadystatechange = handleStateChange;

            xmlHttp.send(content);

            //control.Content.Page.UpdateText(result);
            return result;
        }

        function handleStateChange() {
            if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                result = xmlHttp.responseText;
            }
            else {
                result = "Request failed!";
            }
        }
    </script>

    <script type="text/javascript">
        //在新的标签页tabName打开url链接
        function openNewPage(url, tabName) {
            window.open(url, tabName);
        }
    </script>

</head>
<%--<body onload="GetMap();">--%>
<body>
    <form id="form1" runat="server" style="height:100%">
    <div id="silverlightControlHost">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
		  <param name="source" value="ClientBin/GeoSearch.xap"/>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="5.0.61118.0" />
		  <param name="autoUpgrade" value="true" />
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=5.0.61118.0" style="text-decoration:none">
 			  <img src="http://cisc.gmu.edu/images/top1.gif" alt="GeoSearch: a distributed search broker to facilitate geospatial resources discovery using Microsoft technologies (Windows Azure, Silverlight, PivotViewer, Bing Maps, ASP.NET, WCF RIA) and other Geospatial CyberInfrastructures." style="border-style:none"/>
		  </a>
	    </object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe></div>
    </form>
</body>
</html>
