create table [contact] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[contact]]] primary key,
    [SinkCreatedOn] datetime,
    [SinkModifiedOn] datetime,
    [statecode] int,
    [statuscode] int,
    [address1_shippingmethodcode] int,
    [address2_shippingmethodcode] int,
    [msdyn_orgchangestatus] int,
    [familystatuscode] int,
    [address1_freighttermscode] int,
    [address2_addresstypecode] int,
    [shippingmethodcode] int,
    [preferredappointmenttimecode] int,
    [preferredcontactmethodcode] int,
    [address1_addresstypecode] int,
    [address3_freighttermscode] int,
    [dfeta_title] int,
    [territorycode] int,
    [accountrolecode] int,
    [dfeta_addressverificationflag] int,
    [dfeta_inductionstatus] int,
    [address2_freighttermscode] int,
    [haschildrencode] int,
    [gendercode] int,
    [customertypecode] int,
    [educationcode] int,
    [address3_shippingmethodcode] int,
    [customersizecode] int,
    [paymenttermscode] int,
    [address3_addresstypecode] int,
    [dfeta_suppressionflag] int,
    [preferredappointmentdaycode] int,
    [leadsourcecode] int,
    [isbackofficecustomer] bit,
    [merged] bit,
    [donotfax] bit,
    [followemail] bit,
    [donotemail] bit,
    [isautocreate] bit,
    [donotpostalmail] bit,
    [dfeta_addressconsent] bit,
    [isprivate] bit,
    [dfeta_activesanctions] bit,
    [donotbulkemail] bit,
    [dfeta_disability] bit,
    [donotsendmm] bit,
    [donotphone] bit,
    [creditonhold] bit,
    [marketingonly] bit,
    [donotbulkpostalmail] bit,
    [dfeta_trnrequired] bit,
    [participatesinworkflow] bit,
    [msdyn_gdproptout] bit,
    [originatingleadid] uniqueidentifier,
    [originatingleadid_entitytype] nvarchar(128),
    [dfeta_nationalityid] uniqueidentifier,
    [dfeta_nationalityid_entitytype] nvarchar(128),
    [parentcontactid] uniqueidentifier,
    [parentcontactid_entitytype] nvarchar(128),
    [defaultpricelevelid] uniqueidentifier,
    [defaultpricelevelid_entitytype] nvarchar(128),
    [accountid] uniqueidentifier,
    [accountid_entitytype] nvarchar(128),
    [slaid] uniqueidentifier,
    [slaid_entitytype] nvarchar(128),
    [slainvokedid] uniqueidentifier,
    [slainvokedid_entitytype] nvarchar(128),
    [preferredserviceid] uniqueidentifier,
    [preferredserviceid_entitytype] nvarchar(128),
    [owninguser] uniqueidentifier,
    [owninguser_entitytype] nvarchar(128),
    [preferredsystemuserid] uniqueidentifier,
    [preferredsystemuserid_entitytype] nvarchar(128),
    [parent_contactid] uniqueidentifier,
    [parent_contactid_entitytype] nvarchar(128),
    [dfeta_disabilityid] uniqueidentifier,
    [dfeta_disabilityid_entitytype] nvarchar(128),
    [createdby] uniqueidentifier,
    [createdby_entitytype] nvarchar(128),
    [owningteam] uniqueidentifier,
    [owningteam_entitytype] nvarchar(128),
    [modifiedbyexternalparty] uniqueidentifier,
    [modifiedbyexternalparty_entitytype] nvarchar(128),
    [createdbyexternalparty] uniqueidentifier,
    [createdbyexternalparty_entitytype] nvarchar(128),
    [owningbusinessunit] uniqueidentifier,
    [owningbusinessunit_entitytype] nvarchar(128),
    [modifiedonbehalfby] uniqueidentifier,
    [modifiedonbehalfby_entitytype] nvarchar(128),
    [dfeta_qtsregistrationid] uniqueidentifier,
    [dfeta_qtsregistrationid_entitytype] nvarchar(128),
    [transactioncurrencyid] uniqueidentifier,
    [transactioncurrencyid_entitytype] nvarchar(128),
    [modifiedby] uniqueidentifier,
    [modifiedby_entitytype] nvarchar(128),
    [dfeta_ethnicityid] uniqueidentifier,
    [dfeta_ethnicityid_entitytype] nvarchar(128),
    [masterid] uniqueidentifier,
    [masterid_entitytype] nvarchar(128),
    [createdonbehalfby] uniqueidentifier,
    [createdonbehalfby_entitytype] nvarchar(128),
    [preferredequipmentid] uniqueidentifier,
    [preferredequipmentid_entitytype] nvarchar(128),
    [ownerid] uniqueidentifier,
    [ownerid_entitytype] nvarchar(128),
    [parentcustomerid] uniqueidentifier,
    [parentcustomerid_entitytype] nvarchar(128),
    [aging60] decimal(38, 2),
    [creditlimit_base] decimal(38, 4),
    [aging30] decimal(38, 2),
    [aging30_base] decimal(38, 4),
    [annualincome] decimal(38, 2),
    [aging90_base] decimal(38, 4),
    [creditlimit] decimal(38, 2),
    [aging60_base] decimal(38, 4),
    [aging90] decimal(38, 2),
    [annualincome_base] decimal(38, 4),
    [overriddencreatedon] datetime,
    [dfeta_capitaninumberchangedon] datetime,
    [dfeta_emailaddresslastupdated] datetime,
    [createdbyexternalpartyyominame] nvarchar(100),
    [dfeta_eytsdatechangedon] datetime,
    [address2_telephone3] nvarchar(50),
    [teamsfollowed] int,
    [dfeta_previousemail] nvarchar(200),
    [address2_upszone] nvarchar(4),
    [createdonbehalfbyyominame] nvarchar(100),
    [parentcustomeridtype] nvarchar(4000),
    [parentcustomeridyominame] nvarchar(450),
    [pager] nvarchar(50),
    [mobilephone] nvarchar(60),
    [dfeta_capitadateofbirthchangedon] datetime,
    [accountidyominame] nvarchar(100),
    [numberofchildren] int,
    [mastercontactidyominame] nvarchar(100),
    [address2_stateorprovince] nvarchar(50),
    [createdbyyominame] nvarchar(100),
    [address2_line3] nvarchar(250),
    [assistantname] nvarchar(100),
    [address2_latitude] decimal(38, 5),
    [fax] nvarchar(60),
    [exchangerate] decimal(38, 10),
    [managerphone] nvarchar(50),
    [dfeta_sauserid] nvarchar(20),
    [address1_line3] nvarchar(250),
    [telephone3] nvarchar(50),
    [address3_utcoffset] int,
    [transactioncurrencyidname] nvarchar(100),
    [managername] nvarchar(100),
    [address1_telephone3] nvarchar(50),
    [address2_name] nvarchar(100),
    [address3_postofficebox] nvarchar(20),
    [owningbusinessunitname] nvarchar(160),
    [onholdtime] int,
    [dfeta_contactnumberlastupdated] datetime,
    [dfeta_qtsregistrationidname] nvarchar(250),
    [childrensnames] nvarchar(255),
    [address1_postalcode] nvarchar(20),
    [businesscard] nvarchar(max),
    [address1_line1] nvarchar(250),
    [department] nvarchar(100),
    [createdbyexternalpartyname] nvarchar(100),
    [address3_county] nvarchar(50),
    [preferredequipmentidname] nvarchar(100),
    [dfeta_qtsdatechangedon] datetime,
    [dfeta_trn] nvarchar(50),
    [modifiedbyyominame] nvarchar(100),
    [address3_line1] nvarchar(250),
    [address2_country] nvarchar(80),
    [traversedpath] nvarchar(1250),
    [address3_telephone1] nvarchar(50),
    [address3_telephone2] nvarchar(50),
    [address3_telephone3] nvarchar(50),
    [address1_county] nvarchar(50),
    [owneridyominame] nvarchar(100),
    [dfeta_mergeinfo] nvarchar(100),
    [defaultpricelevelidname] nvarchar(100),
    [websiteurl] nvarchar(200),
    [modifiedbyname] nvarchar(100),
    [mastercontactidname] nvarchar(100),
    [entityimageid] uniqueidentifier,
    [address3_line3] nvarchar(250),
    [address3_country] nvarchar(80),
    [address2_telephone1] nvarchar(50),
    [address2_telephone2] nvarchar(50),
    [modifiedonbehalfbyname] nvarchar(100),
    [parentcustomeridname] nvarchar(160),
    [emailaddress2] nvarchar(100),
    [utcconversiontimezonecode] int,
    [employeeid] nvarchar(50),
    [address3_composite] nvarchar(max),
    [dfeta_previouslastname] nvarchar(50),
    [birthdate] datetime,
    [address1_primarycontactname] nvarchar(100),
    [address1_utcoffset] int,
    [createdon] datetime,
    [address1_longitude] decimal(38, 5),
    [yomifullname] nvarchar(450),
    [telephone2] nvarchar(60),
    [address2_longitude] decimal(38, 5),
    [address2_line1] nvarchar(250),
    [address2_city] nvarchar(80),
    [callback] nvarchar(50),
    [emailaddress3] nvarchar(100),
    [address3_stateorprovince] nvarchar(50),
    [emailaddress1] nvarchar(100),
    [owneridtype] nvarchar(4000),
    [address1_fax] nvarchar(50),
    [preferredsystemuseridname] nvarchar(100),
    [address3_addressid] uniqueidentifier,
    [address2_primarycontactname] nvarchar(100),
    [dfeta_hlta_candidate_id] nvarchar(20),
    [dfeta_dateofdeath] datetime,
    [ftpsiteurl] nvarchar(200),
    [dfeta_disabilityidname] nvarchar(250),
    [address2_postofficebox] nvarchar(20),
    [address3_line2] nvarchar(250),
    [parent_contactidyominame] nvarchar(450),
    [dfeta_loginfailedcounter] int,
    [processid] uniqueidentifier,
    [address3_fax] nvarchar(50),
    [subscriptionid] uniqueidentifier,
    [address2_addressid] uniqueidentifier,
    [governmentid] nvarchar(50),
    [business2] nvarchar(50),
    [middlename] nvarchar(100),
    [address1_city] nvarchar(80),
    [slaname] nvarchar(100),
    [originatingleadidyominame] nvarchar(100),
    [entityimage_timestamp] bigint,
    [modifiedon] datetime,
    [versionnumber] bigint,
    [jobtitle] nvarchar(100),
    [preferredserviceidname] nvarchar(100),
    [dfeta_trnallocaterequest] datetime,
    [dfeta_tssupdate] datetime,
    [address3_upszone] nvarchar(4),
    [address3_city] nvarchar(80),
    [yomilastname] nvarchar(150),
    [address3_latitude] decimal(38, 5),
    [address1_composite] nvarchar(max),
    [contactid] uniqueidentifier,
    [address2_postalcode] nvarchar(20),
    [dfeta_addresslastupdated] datetime,
    [dfeta_capitatrnchangedon] datetime,
    [company] nvarchar(50),
    [dfeta_ethnicityidname] nvarchar(250),
    [yomifirstname] nvarchar(150),
    [address1_stateorprovince] nvarchar(50),
    [suffix] nvarchar(10),
    [address2_utcoffset] int,
    [lastusedincampaign] datetime,
    [address2_fax] nvarchar(50),
    [preferredsystemuseridyominame] nvarchar(100),
    [address1_upszone] nvarchar(4),
    [modifiedbyexternalpartyname] nvarchar(100),
    [address3_primarycontactname] nvarchar(100),
    [address1_telephone1] nvarchar(50),
    [address1_telephone2] nvarchar(50),
    [telephone1] nvarchar(60),
    [address2_line2] nvarchar(250),
    [dfeta_partyid] nvarchar(50),
    [address1_addressid] uniqueidentifier,
    [address3_postalcode] nvarchar(20),
    [spousesname] nvarchar(100),
    [owneridname] nvarchar(100),
    [address1_latitude] decimal(38, 5),
    [address2_composite] nvarchar(max),
    [parentcontactidname] nvarchar(100),
    [createdonbehalfbyname] nvarchar(100),
    [stageid] uniqueidentifier,
    [createdbyname] nvarchar(100),
    [entityimage_url] nvarchar(200),
    [modifiedonbehalfbyyominame] nvarchar(100),
    [address2_county] nvarchar(50),
    [originatingleadidname] nvarchar(100),
    [externaluseridentifier] nvarchar(50),
    [lastonholdtime] datetime,
    [nickname] nvarchar(50),
    [address1_name] nvarchar(200),
    [address1_postofficebox] nvarchar(20),
    [description] nvarchar(max),
    [businesscardattributes] nvarchar(4000),
    [address1_line2] nvarchar(250),
    [timespentbymeonemailandmeetings] nvarchar(1250),
    [dfeta_ninumber] nvarchar(9),
    [importsequencenumber] int,
    [lastname] nvarchar(100),
    [dfeta_evolveid] nvarchar(50),
    [timezoneruleversionnumber] int,
    [parentcontactidyominame] nvarchar(100),
    [address1_country] nvarchar(80),
    [anniversary] datetime,
    [dfeta_husid] nvarchar(100),
    [parent_contactidname] nvarchar(160),
    [address3_longitude] decimal(38, 5),
    [yomimiddlename] nvarchar(150),
    [address3_name] nvarchar(200),
    [fullname] nvarchar(160),
    [firstname] nvarchar(100),
    [dfeta_qtsdate] datetime,
    [salutation] nvarchar(100),
    [accountidname] nvarchar(100),
    [modifiedbyexternalpartyyominame] nvarchar(100),
    [slainvokedidname] nvarchar(100),
    [dfeta_eytsdate] datetime,
    [dfeta_nationalityidname] nvarchar(250),
    [home2] nvarchar(50),
    [assistantphone] nvarchar(50),
    [msdyn_segmentid] uniqueidentifier,
    [msdyn_segmentid_entitytype] nvarchar(128),
    [msdyn_segmentidname] nvarchar(100),
    [dfeta_tspersonid] nvarchar(100)
)
