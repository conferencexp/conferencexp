The following process was used to create the interop wrappers...

1.  Include pre-existing idl files without a library block into a new idl file with a library block
2.  Run MIDL against the new idl file, in order to create a tlb file (type library)
3.  Run TLBIMP against the tlb file generated in step 2 in order to create the interop wrapper dll (be sure and choose the appropriate namespace and output file)
4.  Run ILDASM against the dll in order to disassemble it (turn it into human readable format), File => Dump => uncheck everything but Dump IL Code
5.  Manually adjust the marshaling (data types) in the il file via a text editor and save it
6.  Recompile the il code with ILASM