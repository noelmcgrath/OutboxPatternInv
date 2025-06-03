SELECT TOP (1000) [Id]
      ,[ContinuumOrderIdentifier]
      ,[MerchantOrderIdentifier]
      ,[MerchantId]
      ,[SaleCurrencyId]
      ,[SaleValue]
      ,[ResultCodeId]
      ,[CreationTimestamp]
      ,[VersionSequence]
      ,[OrderSessionId]
  FROM [PCS_v2_at].[dbo].[Lookup]


  SELECT TOP (1000) [Id]
      ,[MessageType]
      ,[MessageJSON]
      ,[OccurredTimestamp]
      ,[ProcessedTimestamp]
      ,[VersionSequence]
      ,[Error]
  FROM [PCS_v2_at].[dbo].[OutboxMessages]

  SELECT TOP (1000) [Id]
      ,[MessageType]
      ,[MessageJSON]
      ,[ReceivedAt]
      ,[ProcessedAt]
      ,[Status]
      ,[ConsumerId]
      ,[ErrorDetails]
      ,[Retries]
  FROM [PCS_v2_at].[dbo].[InboxMessages]

  SELECT Status FROM dbo.InboxMessages WHERE Id = '051edd7f-df47-4d4f-af8e-247fec52af04'


  -- DELETE FROM [PCS_v2_at].[dbo].[OutboxMessages]  
  -- DELETE FROM [PCS_v2_at].[dbo].[Lookup]
  -- DELETE FROM [PCS_v2_at].[dbo].[InboxMessages]


