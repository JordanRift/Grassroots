DROP FUNCTION IF EXISTS grassroots.Emlah_Error_NewSequenceNumber;
CREATE FUNCTION grassroots.`Emlah_Error_NewSequenceNumber`() RETURNS int(11)
    NO SQL
BEGIN
      DECLARE newSequence INT;
      SELECT IFNULL(MAX(`Sequence`) +1 , 1) INTO newSequence FROM `elmah_error`;
      RETURN(newSequence);
END;
