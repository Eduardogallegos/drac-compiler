;; WebAssembly text format code generated by the drac compiler.

(module
	(import "drac" "printi" (func $printi (param i32) (result i32)))
	(import "drac" "printc" (func $printc (param i32) (result i32)))
	(import "drac" "prints" (func $prints (param i32) (result i32)))
	(import "drac" "println" (func $println (result i32)))
	(import "drac" "readi" (func $readi (result i32)))
	(import "drac" "reads" (func $reads (result i32)))
	(import "drac" "new" (func $new (param i32) (result i32)))
	(import "drac" "size" (func $size (param i32) (result i32)))
	(import "drac" "add" (func $add (param i32 i32) (result i32)))
	(import "drac" "get" (func $get (param i32 i32) (result i32)))
	(import "drac" "set" (func $set (param i32 i32 i32) (result i32)))

	(func $is_palindrome
		(param $str i32)
		(result i32)
		(local $_temp i32)
		(local $start i32)
		(local $finish i32)
		i32.const 0
		local.set $start
		local.get $str
		call $size
		i32.const 1
		i32.sub
		local.set $finish
		block $00000
		loop $00001
		local.get $start
		local.get $finish
		i32.lt_s
		i32.eqz
		br_if $00000
		local.get $str
		local.get $start
		call $get
		local.get $str
		local.get $finish
		call $get
		i32.ne
		if
    i32.const 0
		return
		end
		local.get $start
		i32.const 1
		i32.add
		local.set $start
		local.get $finish
		i32.const 1
		i32.sub
		local.set $finish
		br $00001
		end
		end
    i32.const 1
		return
		i32.const 0
	)
	(func
		(export "main")
		(result i32)
		(local $_temp i32)
		(local $str i32)
		(local $option i32)
		block $00002
		loop $00003
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		i32.const 73
		call $add
		drop
		i32.const 110
		call $add
		drop
		i32.const 112
		call $add
		drop
		i32.const 117
		call $add
		drop
		i32.const 116
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 115
		call $add
		drop
		i32.const 116
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 105
		call $add
		drop
		i32.const 110
		call $add
		drop
		i32.const 103
		call $add
		drop
		i32.const 58
		call $add
		drop
		i32.const 32
		call $add
		drop
		call $prints
		drop
		call $reads
		local.set $str
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		i32.const 84
		call $add
		drop
		i32.const 104
		call $add
		drop
		i32.const 101
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 115
		call $add
		drop
		i32.const 116
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 105
		call $add
		drop
		i32.const 110
		call $add
		drop
		i32.const 103
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 34
		call $add
		drop
		call $prints
		drop
		local.get $str
		call $prints
		drop
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		i32.const 34
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 105
		call $add
		drop
		i32.const 115
		call $add
		drop
		i32.const 32
		call $add
		drop
		call $prints
		drop
		local.get $str
		call $is_palindrome
		i32.eqz
		if
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		i32.const 78
		call $add
		drop
		i32.const 79
		call $add
		drop
		i32.const 84
		call $add
		drop
		i32.const 32
		call $add
		drop
		call $prints
		drop
		end
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		i32.const 97
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 112
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 108
		call $add
		drop
		i32.const 105
		call $add
		drop
		i32.const 110
		call $add
		drop
		i32.const 100
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 111
		call $add
		drop
		i32.const 109
		call $add
		drop
		i32.const 101
		call $add
		drop
		i32.const 46
		call $add
		drop
		i32.const 10
		call $add
		drop
		call $prints
		drop
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		i32.const 67
		call $add
		drop
		i32.const 104
		call $add
		drop
		i32.const 101
		call $add
		drop
		i32.const 99
		call $add
		drop
		i32.const 107
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 110
		call $add
		drop
		i32.const 111
		call $add
		drop
		i32.const 116
		call $add
		drop
		i32.const 104
		call $add
		drop
		i32.const 101
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 115
		call $add
		drop
		i32.const 116
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 105
		call $add
		drop
		i32.const 110
		call $add
		drop
		i32.const 103
		call $add
		drop
		i32.const 63
		call $add
		drop
		i32.const 32
		call $add
		drop
		call $prints
		drop
		call $reads
		local.set $option
		local.get $option
		call $size
		i32.const 0
		i32.eq
		if
		i32.const 78
		local.set $option
		else
		local.get $option
		i32.const 0
		call $get
		local.set $option
		end
		local.get $option
		i32.const 89
		i32.eq
		if(result i32)
		i32.const 1
		else
		local.get $option
		i32.const 121
		i32.eq
		i32.eqz
		i32.eqz
		end
		br_if $00003
		end
		end
		i32.const 0
	)
)
