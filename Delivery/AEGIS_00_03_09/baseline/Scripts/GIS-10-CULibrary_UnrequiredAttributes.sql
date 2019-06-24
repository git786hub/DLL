set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\CULibrary_UnrequiredAttributes.sql.log
--**************************************************************************************
--SCRIPT NAME: CULibrary_UnrequiredAttributes.sql
--**************************************************************************************
-- AUTHOR         : Rich Adase
-- DATE           : 22-AUG-2018
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Make all attributes driven by CU non-required (ALM 708)
--**************************************************************************************

 -- AMS Router / Power Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1520102;
 -- Area Light / Lamp Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 610101;
 -- Area Light / Connection Status
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 610106;
 -- Area Light / Wattage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 610102;
 -- Arrestor / Voltage Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 330103;
 -- Arrestor / Class
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 330101;
 -- Autotransformer / Phase Quantity
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 340206;
 -- Autotransformer / Low-side Voltage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 340204;
 -- Autotransformer / High-side Voltage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 340203;
 -- Autotransformer / KVA Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 340202;
 -- Capacitor / Capacitor Connection
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 40110;
 -- Capacitor / Unit Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 40109;
 -- Capacitor / Switch Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 40104;
 -- Capacitor / Unit Quantity
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 40105;
 -- Capacitor / Bank Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 40102;
 -- Capacitor / Control Status
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 40111;
 -- Conduit / Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1040102;
 -- Conduit / Material
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1040101;
 -- Fault Indicator / Trip Current Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 70102;
 -- Foreign Communications Cable / Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 740101;
 -- Fuse Saver / SCADA Capable
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 331000;
 -- Guy / Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1050101;
 -- Guy / Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1050102;
 -- Manhole / Manhole Type Code
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1060101;
 -- Network Protector / Protector Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1570401;
 -- Network Protector / Continuous Current Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1570404;
 -- Pad / Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1080202;
 -- Pad / Material Code
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1080201;
 -- Pole / Owner Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 120;
 -- Pole / Material
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1100104;
 -- Pole / Class
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1100101;
 -- Pole / Height
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1100102;
 -- Primary Conductor - OH / Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 80202;
 -- Primary Conductor - OH / Maximum Recommended Length
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 80205;
 -- Primary Conductor - OH / Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 80203;
 -- Primary Conductor - UG / Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 90204;
 -- Primary Conductor - UG / Insulation Voltage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 90202;
 -- Primary Conductor - UG / Insulation Material
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 90201;
 -- Primary Conductor - UG / Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 90205;
 -- Primary Enclosure / Phase Quantity
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 50101;
 -- Primary Enclosure / Continuous Current Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 50102;
 -- Primary Fuse - OH / Link Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 110103;
 -- Primary Fuse - OH / Link Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 110102;
 -- Primary Fuse - OH / Interrupt Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 110202;
 -- Primary Fuse - UG / SCADA Capable
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 331000;
 -- Primary Point of Delivery / Delivery Class
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 120101;
 -- Primary Pull Box / Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1090401;
 -- Primary Pull Box / Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1090402;
 -- Primary Pull Box / Material
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1090403;
 -- Primary Splice - Network / Connection Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 7201;
 -- Primary Splice - Network / Splice Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 72010;
 -- Primary Switch - OH / Switch Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 130103;
 -- Primary Switch - OH / SCADA Capable
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 331000;
 -- Primary Switch - OH / Continuous Current Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 130203;
 -- Primary Switch - OH / Load Break
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 130105;
 -- Primary Switch Gear / Facility Front
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 190108;
 -- Primary Switch Gear / Phase Quantity
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 190107;
 -- Primary Switch Gear / Interrupt Medium
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 190106;
 -- Primary Switch Gear / Continuous Current Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 190103;
 -- Primary Switch Gear / Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 190101;
 -- Rack / Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1110101;
 -- Rack / Material
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1110102;
 -- Rack / Weight Class
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1110103;
 -- Recloser - OH / Interrupt Medium
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 140212;
 -- Recloser - OH / Phase Quantity
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 140114;
 -- Recloser - OH / Interrupt Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 140208;
 -- Recloser - OH / Continuous Current Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 140204;
 -- Recloser - OH / Coil Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 140202;
 -- Recloser - OH / Recloser Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 140215;
 -- Riser / Conduit Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1120102;
 -- Riser / Riser Material Type Code
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1120103;
 -- Secondary Box / Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1130101;
 -- Secondary Box / Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1130102;
 -- Secondary Box / Material
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1130103;
 -- Secondary Box / Use Code
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1130104;
 -- Secondary Breaker / Continuous Current Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1540203;
 -- Secondary Breaker / Interrupt Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1540204;

update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1540205;
 -- Secondary Conductor - OH / Conductor Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 530101;
 -- Secondary Conductor - OH / Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 530201;
 -- Secondary Conductor - OH / Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 530202;
 -- Secondary Conductor - OH Network / Bundle
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 530205;
 -- Secondary Enclosure / Phase Quantity
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1200201;
 -- Secondary Enclosure / Continuous Current Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1200202;
 -- Secondary Fuse / Link Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1610204;
 -- Secondary Fuse / Interrupt Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1610202;
 -- Secondary Fuse / Link Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1610203;
 -- Secondary Fuse / Continuous Current Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1610201;
 -- Street Light / Luminaire Style
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 560118;
 -- Street Light / Bracket Length
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 560109;
 -- Street Light / Wattage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 560103;
 -- Street Light / Lamp Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 560102;
 -- Street Light Control / Control Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 580100;
 -- Street Light Control / Wattage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 580101;
 -- Street Light Standard / Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1140101;
 -- Street Light Standard / Foundation Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1140103;
 -- Street Light Standard / Height
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1140104;
 -- Street Light Standard / Material
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 1140102;
 -- Transformer - OH / Facility Front
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 590210;
 -- Transformer - OH / Phase Quantity
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 590207;
 -- Transformer - OH / Low-side Voltage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 590205;
 -- Transformer - OH / High-side Voltage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 590204;
 -- Transformer - OH / KVA Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 590202;
 -- Transformer - OH / Bank Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 590101;
 -- Transformer - OH / Protection Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 590209;
 -- Transformer - UG / Feed Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600212;
 -- Transformer - UG / Phase Quantity
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600207;
 -- Transformer - UG / Secondary Voltage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600205;
 -- Transformer - UG / Primary Voltage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600204;
 -- Transformer - UG / KVA Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600202;
 -- Transformer - UG / Facility Front
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600210;
 -- Transformer - UG Network / KVA Size
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600202;
 -- Transformer - UG Network / Primary Voltage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600204;
 -- Transformer - UG Network / Secondary Voltage
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600205;
 -- Transformer - UG Network / Phase Quantity
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600207;
 -- Transformer - UG Network / Protection Type
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600209;
 -- Transformer - UG Network / Facility Front
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 600210;
 -- Voltage Regulator / KVA
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 360202;
 -- Voltage Regulator / Control Code
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 360203;
 -- Voltage Regulator / Reverse Current Relay
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 360206;
 -- Voltage Regulator / Internal Surge Arrestor
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 360205;
 -- Voltage Regulator / Continuous Current Rating
update G3E_ATTRIBUTE set G3E_REQUIRED = 0 where G3E_ANO = 360204;

commit;

spool off;
