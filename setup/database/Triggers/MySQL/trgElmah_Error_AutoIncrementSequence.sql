CREATE DEFINER=`root`@`localhost` TRIGGER `grassroots`.`trgElmah_Error_AutoIncrementSequence` BEFORE INSERT ON grassroots.elmah_error FOR EACH ROW
SET NEW.`Sequence` = Emlah_Error_NewSequenceNumber();