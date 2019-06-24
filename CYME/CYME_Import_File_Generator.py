#!/usr/bin/env python
"""
This program's purpose is to produce CYME import files from JSON data in HDS.
"""

import os
import datetime

import cx_Oracle
import pyodbc
import json

__author__ = "Josh Mascorro"
__version__ = "1.0.0"
__status__ = "Development"


# Global variables/constants
# DSN Name
DNS_NAME = "HDSBFX"
# main query
STATEMENT = "select feeder, section_name, header_name, file_type, field_values::JSON::LVARCHAR from cyme_json_feeder order by feeder, header_name"
# system table query
STATEMENT_SYS = "select version_no, revision_no, as_of_dt, system_unit, temp_si, effective_dt from dgs_cyme_system"

# Main Method
def main():
    # main query
    sections = extract(DNS_NAME, STATEMENT)
    # Collect System info
    system_info = get_system(DNS_NAME, STATEMENT_SYS)
    # Add sections to each feeder
    feeders = create_feeders(sections)
    # Convert json_strings to be python-readable
    convert(feeders)
    # Write feeder files
    write(feeders, system_info)

class Section:
    def __init__(self, feeder, section_name, attributes, header_name, file_type, field_values_1, field_values_2, json_obj):
        self.feeder = feeder
        self.section_name = section_name
        self.attributes = attributes
        self.header_name = header_name
        self.file_type = file_type
        self.field_values_1 = field_values_1
        self.field_values_2 = field_values_2
        self.json_obj = json_obj

class Feeder:
    def __init__(self, feeder, sections):
        self.feeder = feeder
        self.sections = sections

class System:
    def __init__(self, version_no, revision_no, as_of_dt, system_unit, temp_si, effective_dt):
        self.version_no = version_no
        self.revision_no = revision_no
        self.as_of_dt = as_of_dt
        self.system_unit = system_unit
        self.temp_si = temp_si
        self.effective_dt = effective_dt

def extract(DSN_Name, Statement):
    cnxn = pyodbc.connect(dsn=DSN_Name)
    cursor = cnxn.cursor()
    cursor.execute(Statement)
    sections = []
    # create sections without the field_values
    for result in cursor:
        # 0 - Feeder
        feeder = result[0]
        # 1 - Section_Name
        section_name = result[1]
        # 2 - Header_Name
        header_name = result[2]
        # 3 - File_Type
        file_type = result[3]
        # Field_Values
        field_values_1 = ""
        field_values_2 = ""
        # JSON
        json_obj = ""
        # Create a record
        section = Section(feeder, section_name, "", header_name, file_type, field_values_1, field_values_2,json_obj)
        # Add record to record list
        sections.append(section)
    # Run a query for each field_values value individually as to not overload the memory limitations of pyodbc
    for sec in sections:
        cursor.execute("select field_values::JSON::LVARCHAR from cyme_json_feeder where feeder = '" + sec.feeder + "' and header_name = '" + sec.header_name +"'")
        for result in cursor:
            # Load json into field_values attribute
            sec.field_values_1 = json.loads(result[0])
        
    return sections

def create_feeders(Sections):
    feeder_names = []
    feeders = []
    section_feeder = []
    for sec in Sections:
        feeder_names.append(sec.feeder)
    # Get unique feeders
    feeder_names = set(feeder_names)
    print(str(feeder_names))
    for feeder_name in feeder_names:
        sections = []
        for sec in Sections:
            section_i = sec
            if sec.header_name == 'FORMAT_FEEDER':
                format_feeder = sec
                continue
            elif sec.header_name == 'FORMAT_SECTION':
                format_section = sec
                continue
            if sec.feeder == feeder_name:
                sections.append(sec)
        sections.append(Section(section_i.feeder, "SECTION", "", ["FORMAT_FEEDER","FORMAT_SECTION"], section_i.file_type, format_feeder.field_values_1, format_section.field_values_1, ""))
        feeder = Feeder(feeder_name, sections)
        feeders.append(feeder)
    print(str(len(feeders)))
    return feeders

def convert(Feeders):
    for f in Feeders:
        sections = f.sections
        for sec in sections:
            json_list = []
            json_list.append(sec.field_values_1)
            #print(str(sec.field_values_1))
            if sec.field_values_2 != "":
                json_list.append(sec.field_values_2)
            else:
                json_list.append("")
            sec.json_obj = json_list

def get_system(DSN_Name, Statement):
    cnxn = pyodbc.connect(dsn=DSN_Name)
    cursor = cnxn.cursor()
    cursor.execute(Statement)
    for result in cursor:
        version_no = result[0]
        revision_no = result[1]
        as_of_dt = result[2]
        system_unit = result[3]
        temp_si = result[4]
        effective_dt = result[5]
        sys = System(version_no, revision_no, as_of_dt, system_unit, temp_si, effective_dt)
    return sys

def write(Feeders, System_Info):
    print(str(len(Feeders)))

    for feeder in Feeders:
        f = open(feeder.feeder + ".SPN.txt", "w+")
        lines = []
        now = datetime.datetime.now()
        system_unit = System_Info.system_unit
        lines.append('[GENERAL]')
        lines.append(now.strftime("DATE=%B %d, %Y at %H:%M:%S"))
        lines.append("CYMDIST_VERSION={}".format(System_Info.version_no))
        lines.append("CYMDIST_REVISION={}".format(System_Info.revision_no))
        lines.append('')
        lines.append("[" + str(system_unit) + "]")
        if system_unit == 'IMPERIAL':
            lines.append("TemperatureInSI={}".format(System_Info.temp_si))
            lines.append('')
        else:
            lines.append('')
        
        print(str(feeder))
        for section in feeder.sections:
            sec_index = 0
            attributes = ""
            lines.append('[' + section.section_name + ']')
            index_sec = len(section.json_obj[0]["fieldlist"])
            index_sec_values = len(section.json_obj[0]["fieldlist"][0]["valuelist"])
            values = []
            values_all = []
            # Collect all of the attributes
            # for i in range(0,len(my_list)):
            #     print(str([j["value"] for j in my_list[i]["valuelist"]]))
            for i in range(0,index_sec):
                attributes += str(section.json_obj[0]["fieldlist"][i]["name"])
                if i != index_sec:
                    attributes += ','
                for i in range(0,index_sec_values - 1):
                    #values.append([j[i] for j in section.json_obj[0]["fieldlist"]])
                    print(str([j for j in section.json_obj[0]["fieldlist"]]))
                values_all.append(values)
            # Write all of the attributes
            lines.append("{}={}".format(section.header_name,attributes))
            # Write all of the values
            for i in range(0, index_sec_values - 1):
                value_row = ','.join([j[i] for j in values_all])
                lines.append(value_row)
            #lines.append("[values here]")
            lines.append("")
            sec_index += 1

        for line in lines:
            f.write(line)
            f.write("\n")
        f.close()

if __name__ == '__main__':
    main()